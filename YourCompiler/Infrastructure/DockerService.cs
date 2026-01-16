using Docker.DotNet;
using Docker.DotNet.Models;
using YourCompiler.Domain;
using YourCompiler.Infrastructure.Interfaces;

namespace YourCompiler.Infrastructure
{
    public class DockerService : IDockerService
    {
        public async Task<CompilerResult> runContainer(ContainerDetails details)
        {
            DockerClient client = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

            // Temporary folder
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            string filePath = Path.Combine(tempDir, $"{details.FileName}.{details.LanguageExtension}");
            await System.IO.File.WriteAllTextAsync(filePath, details.Code);

            // Create container
            var container = await client.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = details.ImageName,
                Cmd = details.ExecutionCommand,
                AttachStdout = true,
                AttachStderr = true,
                HostConfig = new HostConfig
                {
                    AutoRemove = false,
                    Binds = new List<string> { $"{tempDir}:/app" }
                }
            });

            // Start container
            await client.Containers.StartContainerAsync(container.ID, null);

            // Wait for container to finish
            var waitResponse = await client.Containers.WaitContainerAsync(container.ID);

            // Get logs after it finished
            var logs = await client.Containers.GetContainerLogsAsync(
                container.ID,
                false,
                new ContainerLogsParameters
                {
                    ShowStdout = true,
                    ShowStderr = true,
                }
            );

            var (stdout, stderr) = await logs.ReadOutputToEndAsync(CancellationToken.None);

            // Delete container
            await client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters());


            CompilerResult result = new CompilerResult(stdout, stderr);

            // Cleanup
            Directory.Delete(tempDir, true);

            return result;
        }
    }
}
