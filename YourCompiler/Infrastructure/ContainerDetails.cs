using System.Security.Cryptography.X509Certificates;

namespace YourCompiler.Infrastructure
{
    public record ContainerDetails(
        string ImageName,
        string FileName,
        string LanguageExtension,
        List<string> ExecutionCommand,
        string code
     );
}
