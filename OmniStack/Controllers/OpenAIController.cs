using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WMB.Api.Models;
using WMB.Api.Services;

namespace WMB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public OpenAIController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("get-response")]
        public async Task<IActionResult> GetAIResponse([FromBody] AIRequest request)
        {
            if (string.IsNullOrEmpty(request.ApiKey))
                return BadRequest("API Key is required.");

            if (string.IsNullOrEmpty(request.Prompt))
                return BadRequest("Prompt is required.");

            try
            {
                var response = await _openAIService.GetResponseAsync(request.ApiKey, request.Prompt);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

