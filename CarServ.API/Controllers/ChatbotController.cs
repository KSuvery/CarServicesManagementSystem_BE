using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarServ.Service.Services;
using CarServ.Service.Services.Interfaces;

namespace CarServ.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] string userInput)
        {
            var response = await _chatbotService.GetChatbotResponseAsync(userInput);
            return Ok(response);
        }
    }
}
