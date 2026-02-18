namespace YourCompiler.Infrastructure
{ 
    public record AvailableLanguagesDTO(
        Dictionary<string, string> LanguagesToDefaultVersionMap,
        Dictionary<string, string[]> LanguagesToAvailableVersionMap
    );
}
