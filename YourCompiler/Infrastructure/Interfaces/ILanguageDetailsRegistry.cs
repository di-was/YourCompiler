namespace YourCompiler.Infrastructure.Interfaces
{
    public interface ILanguageDetailsRegistry
    {
        public abstract LanguageConfig Resolve(string language);
        public abstract AvailableLanguagesDTO GetAvailableLanguages();
    }
}
