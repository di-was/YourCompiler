
using YourCompiler.Domain;

namespace YourCompiler.Infrastructure.Interfaces
{
    public interface IDockerService
    {
        public abstract Task<CompilerResult> runContainer(LanguageConfig details, string code, string version);
    }
}
