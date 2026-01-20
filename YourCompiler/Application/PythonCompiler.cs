using YourCompiler.Domain;
using YourCompiler.Infrastructure;
using YourCompiler.Infrastructure.Interfaces;

namespace YourCompiler.Application
{
    public class PythonCompiler : ICompiler
    {
        private readonly IDockerService _dockerService;
        private readonly IContainerDetailsRegistry _containerDetailsRegistry;
        private const string languageKey = "python";

        public PythonCompiler(IDockerService dockerService, IContainerDetailsRegistry containerDetailsRegistry) { 
            this._dockerService = dockerService;
            this._containerDetailsRegistry = containerDetailsRegistry;
        }
        public CompilerResult Compile(string code) {

            ContainerDetails details = _containerDetailsRegistry.Resolve(languageKey) with { Code = code};

            CompilerResult result = _dockerService.runContainer(details).Result;
            return result;
        }
    }
}
