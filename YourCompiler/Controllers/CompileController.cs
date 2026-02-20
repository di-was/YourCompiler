using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using YourCompiler.Application;
using YourCompiler.Domain;
using YourCompiler.DTOs.InternalDTOs;
using YourCompiler.DTOs.RequestDTOs;
using Microsoft.AspNetCore.RateLimiting;

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
        [EnableRateLimiting("Compile")]
        [RequestSizeLimit(30*1024)]
        public IActionResult Compile(string language, [FromBody] CompileRequest request)
        {
            string code = request.Code;
            string version = request.Version;
            CompilerFactory? compilerFactory = _serviceProvider.GetService<CompilerFactory>();
            if (compilerFactory == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "CompilerFactory service not found.");
            }
            ICompiler compiler = compilerFactory.CreateCompiler(language);
            CompilerResult result = compiler.Compile(code, version);

          

            if (!result.IsSuccess)
            {
                return Ok(result.Error);
            }
            return Ok(result.Output);
        }
    }
}