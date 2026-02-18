using YourCompiler.DTOs.InternalDTOs;

namespace YourCompiler.Infrastructure.Interfaces
{
    public interface IDockerService
    {
        public abstract Task<CompilerResult> runContainer(LanguageConfig details, string code, string version);
    }
}
