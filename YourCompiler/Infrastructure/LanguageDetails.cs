using System.Security.Cryptography.X509Certificates;

namespace YourCompiler.Infrastructure
{

    public record LanguageConfig(
        string languageExtension,
        string[] executionCommand,
        string defaultVersion,
        string[] availableVersions,
        Dictionary<string, string> containerInfo
        );

    public record LanguagesConfig(
        Dictionary<string, LanguageConfig> configs,
        string[] availableLanguages
        
        );
}
