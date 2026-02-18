using YourCompiler.Infrastructure.Interfaces;

namespace YourCompiler.Infrastructure
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
            var languagesToDefaultVersionMap = new Dictionary<string, string>();
            var languagesToAvailableVersionMap = new Dictionary<string, string[]>();
            foreach (var kvp in _languagesConfig.configs)
            {
                string language = kvp.Key;
                LanguageConfig config = kvp.Value;
                languagesToDefaultVersionMap[language] = config.defaultVersion;
                languagesToAvailableVersionMap[language] = config.availableVersions;
            }
            return new AvailableLanguagesDTO(languagesToDefaultVersionMap, languagesToAvailableVersionMap);
        }
    }
}
