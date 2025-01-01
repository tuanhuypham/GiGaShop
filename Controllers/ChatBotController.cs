using Microsoft.AspNetCore.Mvc;
using Gigashop.Services;
using System.Threading.Tasks;
using Gigashop.Data;

namespace Gigashop.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly OpenAIService _openAiService;
        private readonly ApplicationDBContext _context; // DbContext của bạn

        public ChatBotController(OpenAIService openAiService, ApplicationDBContext context)
        {
            _openAiService = openAiService;
            _context = context;
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

            // Gọi phương thức GetChatGptResponseAsync để lấy phản hồi từ ChatGPT
            string response;
            try
            {
                response = await _openAiService.GetChatGptResponseAsync(userInput);
            }
            catch (HttpRequestException)
            {
                return Json("Rất tiếc, hiện hệ thống không thể xử lý yêu cầu do quá tải hoặc hết hạn mức API. Vui lòng thử lại sau.");
            }

            // Lưu tin nhắn người dùng vào cơ sở dữ liệu
            var userId = 1; // Lấy UserID từ Session hoặc cách khác (sử dụng session hoặc model để xác thực người dùng)
            var username = "Khách hàng"; // Lấy tên người dùng từ session hoặc từ yêu cầu

            var message = new ContactMessage
            {
                UserID = userId,
                Name = username,
                Email = "example@gmail.com", // Lấy email người dùng từ session hoặc nhập
                Subject = "Hỗ trợ", // Tiêu đề tin nhắn
                Message = userInput,
                CreatedAt = DateTime.Now
            };

            // Lưu vào cơ sở dữ liệu
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            // Trả về phản hồi cho client (bot)
            return Json(response);
        }
    }

}
