using YourCompiler.domain;

namespace YourCompiler.domain
{
    public interface ICompiler
    {
        public CompilerResult Compile(string code);
    }
}
