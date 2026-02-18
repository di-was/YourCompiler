namespace YourCompiler.Infrastructure
{
    public record languageInfo(
        string defaultVersion,
        string[] availableVersions,
        string skeletonCode
        );
    public record AvailableLanguagesDTO(
        Dictionary<string, languageInfo> config
    );
}
