using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace Gigashop.Services;
public class OpenAIService
{
    private readonly string _apiKey;

    public OpenAIService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> GetChatGptResponseAsync(string userInput)
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        var requestBody = new
        {
            model = "gpt-4o-mini",  // Hoặc "gpt-4" nếu bạn sử dụng GPT-4
            messages = new[]
{
            new { role = "system", content = @"
                Bạn là một trợ lý ảo thông minh, chuyên cung cấp thông tin về các sản phẩm công nghệ như máy tính, laptop, linh kiện và phụ kiện. 
                Bạn trả lời bằng tiếng Việt, cung cấp thông tin chi tiết, rõ ràng, và dễ hiểu. 
                Hãy giúp người dùng đưa ra quyết định mua sắm thông minh.

                Đây là dữ liệu sản phẩm của bạn (được lưu trữ dưới dạng JSON):
                [
                  {
                    ""Name"": ""ASUS ROG Strix G15"",
                    ""Description"": ""Laptop gaming mạnh mẽ"",
                    ""Price"": 35000000,
                    ""Stock"": 15,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""AMD Ryzen 7 5800H"",
                      ""RAM"": ""16GB DDR4"",
                      ""Storage"": ""1TB SSD"",
                      ""GPU"": ""NVIDIA RTX 3060""
                    }
                  },
                  {
                    ""Name"": ""ASUS ZenBook 14 OLED"",
                    ""Description"": ""Laptop siêu mỏng nhẹ"",
                    ""Price"": 27000000,
                    ""Stock"": 20,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i5-1240P"",
                      ""RAM"": ""8GB DDR4"",
                      ""Storage"": ""512GB SSD"",
                      ""Display"": ""14-inch OLED""
                    }
                  },
                  {
                    ""Name"": ""ASUS TUF Dash F15"",
                    ""Description"": ""Laptop gaming bền bỉ"",
                    ""Price"": 25000000,
                    ""Stock"": 12,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i7-12650H"",
                      ""RAM"": ""16GB DDR5"",
                      ""Storage"": ""512GB SSD"",
                      ""GPU"": ""NVIDIA RTX 3050 Ti""
                    }
                  },
                  {
                    ""Name"": ""ASUS VivoBook 15"",
                    ""Description"": ""Laptop văn phòng giá rẻ"",
                    ""Price"": 15000000,
                    ""Stock"": 25,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i3-1115G4"",
                      ""RAM"": ""4GB DDR4"",
                      ""Storage"": ""256GB SSD"",
                      ""Display"": ""15.6-inch FHD""
                    }
                  },
                  {
                    ""Name"": ""ASUS ROG Zephyrus G14"",
                    ""Description"": ""Laptop gaming siêu mỏng"",
                    ""Price"": 40000000,
                    ""Stock"": 10,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""AMD Ryzen 9 5900HS"",
                      ""RAM"": ""16GB DDR4"",
                      ""Storage"": ""1TB SSD"",
                      ""GPU"": ""NVIDIA RTX 3060""
                    }
                  },
                  {
                    ""Name"": ""ASUS ZenBook Flip 13"",
                    ""Description"": ""Laptop màn hình gập 360 độ"",
                    ""Price"": 32000000,
                    ""Stock"": 8,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i7-1165G7"",
                      ""RAM"": ""16GB LPDDR4X"",
                      ""Storage"": ""1TB SSD"",
                      ""Display"": ""13.3-inch OLED Touch""
                    }
                  },
                  {
                    ""Name"": ""ASUS Chromebook C423"",
                    ""Description"": ""Laptop chạy Chrome OS"",
                    ""Price"": 7000000,
                    ""Stock"": 30,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Celeron N3350"",
                      ""RAM"": ""4GB DDR4"",
                      ""Storage"": ""64GB eMMC"",
                      ""Display"": ""14-inch HD""
                    }
                  },
                  {
                    ""Name"": ""ASUS VivoBook S14"",
                    ""Description"": ""Laptop phong cách dành cho sinh viên"",
                    ""Price"": 20000000,
                    ""Stock"": 18,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i5-1135G7"",
                      ""RAM"": ""8GB DDR4"",
                      ""Storage"": ""512GB SSD"",
                      ""Display"": ""14-inch FHD""
                    }
                  },
                  {
                    ""Name"": ""ASUS ExpertBook B9"",
                    ""Description"": ""Laptop doanh nhân cao cấp"",
                    ""Price"": 45000000,
                    ""Stock"": 6,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i7-1260P"",
                      ""RAM"": ""16GB DDR5"",
                      ""Storage"": ""1TB SSD"",
                      ""Weight"": ""880g""
                    }
                  },
                  {
                    ""Name"": ""ASUS ROG Flow X13"",
                    ""Description"": ""Laptop gaming nhỏ gọn và linh hoạt"",
                    ""Price"": 48000000,
                    ""Stock"": 5,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""AMD Ryzen 9 5900HS"",
                      ""RAM"": ""32GB LPDDR4X"",
                      ""Storage"": ""1TB SSD"",
                      ""GPU"": ""NVIDIA RTX 3050 Ti""
                    }
                  },
                  {
                    ""Name"": ""ASUS TUF Gaming F17"",
                    ""Description"": ""Laptop gaming màn hình lớn"",
                    ""Price"": 28000000,
                    ""Stock"": 15,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i7-12700H"",
                      ""RAM"": ""16GB DDR5"",
                      ""Storage"": ""512GB SSD"",
                      ""GPU"": ""NVIDIA RTX 3060"",
                      ""Display"": ""17.3-inch FHD""
                    }
                  },
                  {
                    ""Name"": ""ASUS VivoBook Pro 16X"",
                    ""Description"": ""Laptop cho sáng tạo nội dung"",
                    ""Price"": 34000000,
                    ""Stock"": 10,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""AMD Ryzen 7 5800H"",
                      ""RAM"": ""16GB DDR4"",
                      ""Storage"": ""1TB SSD"",
                      ""GPU"": ""NVIDIA RTX 3050 Ti""
                    }
                  },
                  {
                    ""Name"": ""ASUS ZenBook Duo 14"",
                    ""Description"": ""Laptop màn hình kép"",
                    ""Price"": 39000000,
                    ""Stock"": 8,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i7-1255U"",
                      ""RAM"": ""16GB DDR5"",
                      ""Storage"": ""1TB SSD"",
                      ""Display"": ""14-inch FHD + 12.6-inch secondary touch""
                    }
                  },
                  {
                    ""Name"": ""ASUS ROG Strix SCAR 15"",
                    ""Description"": ""Laptop gaming cao cấp"",
                    ""Price"": 50000000,
                    ""Stock"": 7,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i9-12900H"",
                      ""RAM"": ""32GB DDR5"",
                      ""Storage"": ""1TB SSD"",
                      ""GPU"": ""NVIDIA RTX 3080 Ti""
                    }
                  },
                  {
                    ""Name"": ""ASUS VivoBook Go 14"",
                    ""Description"": ""Laptop nhỏ gọn giá rẻ"",
                    ""Price"": 12000000,
                    ""Stock"": 30,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""AMD Ryzen 3 3250U"",
                      ""RAM"": ""4GB DDR4"",
                      ""Storage"": ""256GB SSD"",
                      ""Display"": ""14-inch HD""
                    }
                  },
                  {
                    ""Name"": ""ASUS ZenBook Pro 15"",
                    ""Description"": ""Laptop cao cấp cho sáng tạo"",
                    ""Price"": 38000000,
                    ""Stock"": 9,
                    ""Category"": ""Laptop"",
                    ""Specifications"": {
                      ""CPU"": ""Intel Core i7-12700H"",
                      ""RAM"": ""16GB DDR5"",
                      ""Storage"": ""1TB SSD"",
                      ""GPU"": ""NVIDIA RTX 3050""
                    }
                  }
                ]

                Nhiệm vụ của bạn:
                1. Khi người dùng hỏi về sản phẩm, tìm các sản phẩm phù hợp từ danh sách.
                2. Trả lời rõ ràng và chi tiết dựa trên thông tin có trong dữ liệu.
                3. Nếu người dùng yêu cầu lọc (theo giá, danh mục, hoặc thông số kỹ thuật), hãy thực hiện và trả lại kết quả phù hợp.
                4. Nếu một sản phẩm hết hàng (Stock = 0), hãy thông báo rõ ràng.

                Bắt đầu trả lời khi nhận được câu hỏi từ người dùng."},
            new { role = "user", content = userInput }
        },
            temperature = 0.7
        };


        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new System.Exception($"OpenAI API Error: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        dynamic responseJson = JsonConvert.DeserializeObject(responseContent);

        return responseJson.choices[0].message.content;
    }
}

public class OpenAIResponse
{
    public Choice[] Choices { get; set; }
}

public class Choice
{
    public string Text { get; set; }
}
