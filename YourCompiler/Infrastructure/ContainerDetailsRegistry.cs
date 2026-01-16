using YourCompiler.Infrastructure.Interfaces;

namespace YourCompiler.Infrastructure
{
    public class ContainerDetailsRegistry : IContainerDetailsRegistry
    {
        private readonly Dictionary<string, ContainerDetails> _languages;
        public ContainerDetailsRegistry(IConfiguration configuration)
        {
            _languages = configuration
                .GetSection("Languages")
                .Get<Dictionary<string, ContainerDetails>>()
                ?? throw new Exception("Languages configuration is missing or invalid");
        }

        public ContainerDetails Resolve(string language)
        {
            ContainerDetails? details =  null;
          
           if (!_languages.TryGetValue(language, out details))
            {
                throw new Exception($"Unsupported {language} Language");
            }

            return details;
        }
    }
}
