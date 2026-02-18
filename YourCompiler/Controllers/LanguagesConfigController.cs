using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourCompiler.Application.Interfaces;
using YourCompiler.DTOs.ResponseDTOs;

namespace YourCompiler.Controllers
{
    [Route("api/getAvailableLanguages")]
    [ApiController]
    public class LanguagesConfigController : ControllerBase
    {
        private readonly ILanguageDetailsRegistry _languageDetailsRegistry;

        public LanguagesConfigController(ILanguageDetailsRegistry languageDetailsRegistry)
        {
            _languageDetailsRegistry = languageDetailsRegistry;
        }

        [HttpGet()]
        public IActionResult getAvailableLanguages()
        {
            AvailableLanguagesDTO res = _languageDetailsRegistry.GetAvailableLanguages();

            return Ok(res);
        }
    }
}
