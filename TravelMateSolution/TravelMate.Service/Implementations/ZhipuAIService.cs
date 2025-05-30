using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TravelMate.Service.Interfaces;

namespace TravelMate.Service.Implementations
{
	public class ZhipuAIService : IZhipuAIService
	{
		private readonly HttpClient _httpClient;
		private const string ApiKey = "084bc5d7a57a453cacced176e4faab2b.qNz6Pkrsx89vmadu";
		private const string ApiUrl = "https://open.bigmodel.cn/api/paas/v4/chat/completions";

		public ZhipuAIService()
		{
			_httpClient = new HttpClient();
		}

		public string InvokeChatCompletion(string userMessage)
		{
			var requestId = "request-id-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

			var requestBody = new
			{
				model = "glm-4-flash",
				stream = false,
				invoke_method = "invoke",  // 必须指定
				request_id = requestId,
				messages = new[]
				{
			new { role = "user", content = userMessage }
		}
			};

			var json = JsonSerializer.Serialize(requestBody);
			var request = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
			{
				Content = new StringContent(json, Encoding.UTF8, "application/json")
			};
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

			try
			{
				var response = _httpClient.Send(request);
				response.EnsureSuccessStatusCode();

				var responseString = response.Content.ReadAsStringAsync().Result;
				Console.WriteLine("原始返回： " + responseString); // ✅ 打印调试

				using var doc = JsonDocument.Parse(responseString);
				var root = doc.RootElement;

				if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
				{
					var message = choices[0].GetProperty("message");
					if (message.TryGetProperty("content", out var content))
					{
						return content.GetString();
					}
				}

				return "未找到 content 字段";
			}
			catch (Exception ex)
			{
				return "响应异常：" + ex.Message;
			}
		}


	}
}
