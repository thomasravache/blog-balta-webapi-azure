using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration config;

        public HomeController(IConfiguration configuration)
        => config = configuration;

        [HttpGet]
        public IActionResult Get() => Ok();

        [HttpGet("v1/auth-by-api-key")]
        [ApiKey]
        public IActionResult AuthByApiKey() => Ok();

        [HttpGet("v1/environment")] // endpoint de teste
        public IActionResult GetEnv(
        // [FromServices] IConfiguration config // pegando configurações do program
        )
        {
            var env = config.GetValue<string>("Env");

            return Ok(new
            {
                environment = env
            });
        }
    }
}