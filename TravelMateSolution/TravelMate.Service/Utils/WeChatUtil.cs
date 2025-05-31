using System.Text.Json;

namespace TravelMate.Service.Utils
{
    public class WeChatUtil
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static async Task<WeChatSessionResponse> GetSessionKeyOrOpenIdAsync(string code)
        {
            try
            {
                // 微信小程序官方接口
                string requestUrl = "https://api.weixin.qq.com/sns/jscode2session";

                // 接口所需参数
                var parameters = new Dictionary<string, string>
                {
                    {"appid", "wx898e6fb434083cf9"},
                    {"secret", "28a0d88336884eaea453be993a21c53f"},
                    {"js_code", code},
                    {"grant_type", "authorization_code"}
                };

                // 构建查询字符串
                var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
                var fullUrl = $"{requestUrl}?{queryString}";

                Console.WriteLine($"Calling WeChat API: {fullUrl}");

                // 发送 HTTP 请求
                var response = await _httpClient.GetAsync(fullUrl);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"WeChat API Response: {content}");

                // 解析 JSON 响应
                var jsonDocument = JsonDocument.Parse(content);
                var root = jsonDocument.RootElement;

                // 检查接口返回的错误码
                if (root.TryGetProperty("errcode", out var errCodeElement))
                {
                    var errorCode = errCodeElement.GetString();
                    var errorMsg = root.TryGetProperty("errmsg", out var errMsgElement)
                        ? errMsgElement.GetString()
                        : "Unknown error";

                    Console.WriteLine($"WeChat API Error: {errorCode} - {errorMsg}");

                    // 返回更详细的错误信息
                    string detailedError = errorCode switch
                    {
                        "40013" => "不合法的 AppID",
                        "40125" => "不合法的密钥",
                        "40029" => "code 无效或已过期",
                        "45011" => "频率限制，请稍后再试",
                        _ => $"微信API错误: {errorCode} - {errorMsg}"
                    };

                    return new WeChatSessionResponse { Success = false, ErrorMessage = detailedError };
                }

                // 成功获取到 openid
                var openid = root.TryGetProperty("openid", out var openidElement)
                    ? openidElement.GetString()
                    : null;

                if (string.IsNullOrEmpty(openid))
                {
                    return new WeChatSessionResponse { Success = false, ErrorMessage = "未能获取到有效的 openid" };
                }

                Console.WriteLine($"Successfully got openid: {openid}");

                return new WeChatSessionResponse
                {
                    Success = true,
                    OpenId = openid
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WeChat API Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new WeChatSessionResponse { Success = false, ErrorMessage = $"网络请求异常: {ex.Message}" };
            }
        }
    }

    public class WeChatSessionResponse
    {
        public bool Success { get; set; }
        public string OpenId { get; set; }
        public string ErrorMessage { get; set; }
    }
}