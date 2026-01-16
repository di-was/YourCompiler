using YourCompiler.Domain;
using YourCompiler.Infrastructure;
using YourCompiler.Infrastructure.Interfaces;

namespace YourCompiler.Application
{
    public class CompilerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CompilerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ICompiler CreateCompiler(string language)
        {

            return _serviceProvider.GetRequiredKeyedService<ICompiler>(language.ToLower());
        }
    }
}
