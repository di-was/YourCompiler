using YourCompiler.Domain;

namespace YourCompiler.Application
{
    public class PythonCompiler : ICompiler
    {
        public CompilerResult Compile(string code) {
            CompilerResult result = new CompilerResult("Hello World", null);
            return result;
        }
    }
}
