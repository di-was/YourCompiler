using YourCompiler.DTOs.ResponseDTOs;
using YourCompiler.DTOs.InternalDTOs;
using YourCompiler.Application.Interfaces;

namespace YourCompiler.Application
{
    public class LanguageDetailsRegistry : ILanguageDetailsRegistry
    {
        private readonly LanguagesConfig _languagesConfig;
        public LanguageDetailsRegistry(IConfiguration configuration)
        {
            _languagesConfig = configuration
                .GetSection("Languages")
                .Get<LanguagesConfig>()
                ?? throw new Exception("Languages configuration is missing or invalid");
        }

        public LanguageConfig Resolve(string language)
        {
            LanguageConfig? details = null;

            foreach (var kvp in _languagesConfig.configs)
            {
                Console.WriteLine($"key: {kvp.Key}, value: {kvp.Value}");
            }
            Console.WriteLine(_languagesConfig.configs.Count);

            if (!_languagesConfig.configs.TryGetValue(language, out details))
            {
                throw new Exception($"Unsupported {language} Language");
            }

            return details;
        }

        public AvailableLanguagesDTO GetAvailableLanguages()
        {
            var res = new Dictionary<string, languageInfo>();
            foreach (var kvp in _languagesConfig.configs)
            {
                string language = kvp.Key;
                LanguageConfig config = kvp.Value;
                res[language] = new languageInfo
                (
                    defaultVersion: config.defaultVersion,
                    availableVersions: config.availableVersions,
                    skeletonCode: config.skeletonCode
                );

            }
            return new AvailableLanguagesDTO(res);
        }
    }
}
