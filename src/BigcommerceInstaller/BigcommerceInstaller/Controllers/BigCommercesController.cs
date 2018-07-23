using BigcommerceInstaller.Models;
using BigcommerceInstaller.Servcices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BigcommerceInstaller.Controllers
{
    [Route("api/[controller]")]
    public class BigCommercesController : Controller
    {
        private readonly IBigCommerceService _bigCommerceService;
        private readonly ILogger<BigCommercesController> _logger;
        private readonly IHostingEnvironment _env;

        public BigCommercesController(IBigCommerceService bigCommerceService, ILogger<BigCommercesController> logger, IHostingEnvironment env)
        {
            _bigCommerceService = bigCommerceService ?? throw new ArgumentNullException(nameof(bigCommerceService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpGet("oauth")]
        public async Task<IActionResult> OAuth(AuthRequest authRequest)
        {
            _logger.LogInformation(_env.EnvironmentName);

            _logger.LogInformation("OAuthing...");

            _logger.LogInformation($"store_hash: {authRequest.GetStoreHash()}");

            await _bigCommerceService.DoAuthAsync(authRequest);

            string content = $@"
                <!DOCTYPE html>
                <html>
                    <head>
                        <title>Page Title</title>
                    </head>
                    <body>
                        <p>Successfull installed Chatigy.</p>
                    </body>
                </html>
            ";

            return new ContentResult()
            {
                Content = content,
                ContentType = "text/html",
            };
        }

        [HttpGet("load")]
        public async Task<IActionResult> Load(string signed_payload)
        {            
            _logger.LogInformation(signed_payload);

            var html = await _bigCommerceService.DoLoadAsync(signed_payload);

            return new ContentResult()
            {
                Content = html,
                ContentType = "text/html",
            };
        }

        [HttpGet("uninstall")]
        public async Task<IActionResult> Uninstall()
        {
            _logger.LogInformation("uninstalling...");

            return Ok();
        }
    }
}
