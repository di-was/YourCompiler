namespace YourCompiler.DTOs.RequestDTOs
{
    public record CompileRequest
    {
        public string Code { get; init; }
        public string Version { get; init; } = "defaultVersion";
    }
}
