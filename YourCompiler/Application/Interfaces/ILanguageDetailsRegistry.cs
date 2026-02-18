using YourCompiler.DTOs.InternalDTOs;
using YourCompiler.DTOs.ResponseDTOs;

namespace YourCompiler.Application.Interfaces
{
    public interface ILanguageDetailsRegistry
    {
        public abstract LanguageConfig Resolve(string language);
        public abstract AvailableLanguagesDTO GetAvailableLanguages();
    }
}
