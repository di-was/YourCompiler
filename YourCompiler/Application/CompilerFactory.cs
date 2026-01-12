using YourCompiler.Domain;

namespace YourCompiler.Application
{
    public class CompilerFactory
    {
        public static ICompiler CreateCompiler(string language)
        {
            return language.ToLower() switch
            {
                "python" => new PythonCompiler(),
                "javascript" => new JavaScriptCompiler(),
                _ => throw new NotSupportedException($"Language '{language}' is not supported.")
            };
        }
    }
}
