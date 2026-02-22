using YourCompiler.Application.Interfaces;
using YourCompiler.Domain;
using YourCompiler.DTOs.InternalDTOs;
using YourCompiler.Infrastructure.Interfaces;

namespace YourCompiler.Application
{
    public class CppCompiler : ICompiler
    {
        private readonly IDockerService _dockerService;
        private readonly ILanguageDetailsRegistry _languageDetailsRegistry;
        private const string languageKey = "cpp";

        public CppCompiler(IDockerService dockerService, ILanguageDetailsRegistry languageDetailsRegistry)
        {
            this._dockerService = dockerService;
            this._languageDetailsRegistry = languageDetailsRegistry;
        }
        public CompilerResult Compile(string code, string version)
        {

            LanguageConfig details = _languageDetailsRegistry.Resolve(languageKey);

            string versionImage = details.containerInfo.GetValueOrDefault(version, details.containerInfo[details.defaultVersion]);

            CompilerResult result = _dockerService.runContainer(details, code, versionImage).Result;
            return result;
        }
    }
}
