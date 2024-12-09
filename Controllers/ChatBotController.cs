using Microsoft.AspNetCore.Mvc;
using Gigashop.Services;
using System.Threading.Tasks;

namespace Gigashop.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly OpenAIService _openAiService;

        public ChatBotController(OpenAIService openAiService)
        {
            _openAiService = openAiService;
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetResponse([FromBody] string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                return Json("Vui lòng nhập câu hỏi.");
            }

            // Gọi phương thức GetChatGptResponseAsync thay vì GetResponseAsync
            var response = await _openAiService.GetChatGptResponseAsync(userInput);

            try
            {
                response = await _openAiService.GetChatGptResponseAsync(userInput);
                return Json(response);
            }
            catch (HttpRequestException ex)
            {
                return Json("Rất tiếc, hiện hệ thống không thể xử lý yêu cầu do quá tải hoặc hết hạn mức API. Vui lòng thử lại sau.");
            }

            return Json(response);  // Trả kết quả dưới dạng JSON
        }
    }
}
