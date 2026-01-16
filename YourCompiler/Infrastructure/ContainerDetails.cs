using System.Security.Cryptography.X509Certificates;

namespace YourCompiler.Infrastructure
{
    public record ContainerDetails(
        string ImageName,
        string LanguageExtension,
        string FileName,
        List<string> ExecutionCommand,
        string Code = ""
     );
}
