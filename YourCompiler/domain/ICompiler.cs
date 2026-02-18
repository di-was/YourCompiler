using YourCompiler.DTOs.InternalDTOs;

namespace YourCompiler.Domain
{
    public interface ICompiler
    {
        public CompilerResult Compile(string code, string version);
    }
}
