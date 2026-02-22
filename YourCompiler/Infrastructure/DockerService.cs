using Docker.DotNet;
using Docker.DotNet.Models;
using YourCompiler.DTOs.InternalDTOs;
using YourCompiler.Infrastructure.Interfaces;
using System.Runtime.InteropServices;

namespace YourCompiler.Infrastructure
{
    public class DockerService : IDockerService
    {
        // Sandbox limitations
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
        private const long MemoryLimit = 256 * 1024 * 1024;
        private const long MemorySwapLimit = 256 * 1024 * 1024;
        private const long NanoCPUs = 1_000_000_000;
        private const long PidsLimit = 32;
        public async Task<CompilerResult> runContainer(LanguageConfig details, string code, string versionImage)
        {
            // Execution Directory and Sandbox Identifier
            string? tempDir = null;
            string? containerId = null;

            DockerClient client = new DockerClientConfiguration(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
                ? new Uri("npipe://./pipe/docker_engine")
                : new Uri("unix:///var/run/docker.sock")
                ).CreateClient();

            try
            {
                // Temporary folder creation and code injection
                tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(tempDir);

                var filePath = Path.Combine(tempDir, $"main.{details.languageExtension}");
                await File.WriteAllTextAsync(filePath, code);


                var hostConfig = new HostConfig
                {
                    AutoRemove = false,

                    // resource limits
                    Memory = MemoryLimit,
                    MemorySwap = MemorySwapLimit,
                    NanoCPUs = NanoCPUs,
                    PidsLimit = PidsLimit,

                    // block network access
                    NetworkMode = "none",

                    // I/O isolation
                    ReadonlyRootfs = false,
                    Binds = new List<string> { $"{tempDir}:/app" },

                    Tmpfs = new Dictionary<string, string>
                    {
                        { "/tmp", "rw,noexec,nosuid,size=64m" }
                    }
                };

                // create container
                var createResponse = await client.Containers.CreateContainerAsync(new CreateContainerParameters
                {
                    Image = versionImage,
                    Cmd = details.executionCommand,
                    WorkingDir = "/app",
                    AttachStdout = true,
                    AttachStderr = true,
                    User = "nobody",
                    HostConfig = hostConfig
                });

                containerId = createResponse.ID;

                // start container
                await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());

                // timout enforcement
                var startedAt = DateTime.UtcNow;

                var waitTask = client.Containers.WaitContainerAsync(containerId);
                var timeoutTask = Task.Delay(DefaultTimeout);

                var completed = await Task.WhenAny(waitTask, timeoutTask);
                var elapsedMs = (DateTime.UtcNow - startedAt).TotalMilliseconds;

                bool timedOut = completed == timeoutTask;

                if (timedOut)
                {
                    Console.WriteLine("timeout");
                    try
                    {
                        await client.Containers.StopContainerAsync(containerId, new ContainerStopParameters { WaitBeforeKillSeconds = 1 });
                    }
                    catch
                    {
                        // forcefully kill if doesn't stop gracefully
                 
                    }
                    try
                    {
                        var inspect = await client.Containers.InspectContainerAsync(containerId);
                        if (inspect?.State?.Running == true)
                        {
                            await client.Containers.KillContainerAsync(containerId, new ContainerKillParameters());
                        }
                    }
                    catch
                    {
                        // ignore - cleanup will force remove
                    }
                }
                


                // retrieve logs
                var logsStream = await client.Containers.GetContainerLogsAsync(
                    containerId,
                    false,
                    new ContainerLogsParameters
                    {
                        ShowStderr = true,
                        ShowStdout = true,
                        Tail = "2000"
                    }
                    );

                using var logCts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                var (stdout, stderr) = await logsStream.ReadOutputToEndAsync(CancellationToken.None);
                var elaspedMs = (DateTime.UtcNow - startedAt).TotalMilliseconds;
                if (timedOut)
                {
                    stderr = (stderr ?? "") + $"\n[debug] timedOut={timedOut}, timeoutMs={DefaultTimeout.TotalMilliseconds}, elapsedMs={elapsedMs}";
                } 

                

                return new CompilerResult(stdout, stderr);
            }
            finally
            {
                if (!string.IsNullOrEmpty(containerId))
                {
                    try
                    {
                        // Force remove if still exists
                        await client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters { Force = true });
                    }
                    catch
                    {
                        // potential log for future use cases
                    }


                }

                if (!string.IsNullOrEmpty(tempDir))
                {
                    try
                    {
                        Directory.Delete(tempDir, recursive: true);

                    }
                    catch
                    {
                        // potential log for future use cases


                    }
                }

            }
        }
    }
}
