using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using YourCompiler.Application;
using YourCompiler.Domain;

namespace YourCompiler.Controllers
{
    [Route("api/compile")]
    [ApiController]
    public class CompileController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        public CompileController(IServiceProvider serviceProvider)

        {
            this._serviceProvider = serviceProvider;
        }
        [HttpPost("{language}")]
        public IActionResult Compile(string language, [FromBody] string code)
        {
            CompilerFactory? compilerFactory = _serviceProvider.GetService<CompilerFactory>();
            if (compilerFactory == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "CompilerFactory service not found.");
            }
            ICompiler compiler = compilerFactory.CreateCompiler(language);
            CompilerResult result = compiler.Compile(code);

            if (!result.IsSuccess)
            {
                return Ok(result.Error);
            }
            return Ok(result.Output);
        }
    }
}