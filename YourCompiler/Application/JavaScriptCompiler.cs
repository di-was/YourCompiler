using YourCompiler.Domain;

namespace YourCompiler.Application
{
    public class JavaScriptCompiler : ICompiler
    {
        public CompilerResult Compile(string code) {
            CompilerResult result = new CompilerResult("Hello World from JavaScript", null);
            return result;
        }
    }
}
