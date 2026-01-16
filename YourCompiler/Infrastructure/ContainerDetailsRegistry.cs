using YourCompiler.Infrastructure.Interfaces;

namespace YourCompiler.Infrastructure
{
    public class ContainerDetailsRegistry : IContainerDetailsRegistry
    {
        private readonly Dictionary<string, ContainerDetails>? _languages;
        public ContainerDetailsRegistry(IConfiguration configuration)
        {
            _languages = configuration
                .GetSection("Languages")
                .Get<Dictionary<string, ContainerDetails>>();
        }

        public ContainerDetails Resolve(string language)
        {
            ContainerDetails? details =  null;

           if (_languages != null && !_languages.TryGetValue(language, out details))
            {
                throw new Exception("Unsupported Language");
            }

            return details;
        }
    }
}
