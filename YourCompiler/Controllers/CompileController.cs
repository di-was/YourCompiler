using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YourCompiler.Controllers
{
    [Route("api/compile")]
    [ApiController]
    public class CompileController : ControllerBase
    {
        [HttpPost("{language}")]
        public IActionResult Compile(string language, [FromBody] string code)
        {
            return Ok($"{language} Compile endpoint is compiling {code}");

        }
    }
}