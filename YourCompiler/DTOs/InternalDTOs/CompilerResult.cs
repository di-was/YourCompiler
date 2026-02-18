namespace YourCompiler.DTOs.InternalDTOs
{
    public record CompilerResult(
        string? Output,
        string? Error
    )
    {
        public bool IsSuccess => string.IsNullOrEmpty(Error);
    }
}
