using YourCompiler.Domain;

namespace YourCompiler.Domain
{
    public interface ICompiler
    {
        public CompilerResult Compile(string code, string version);
    }
}
