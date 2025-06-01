using Microsoft.EntityFrameworkCore;
using TravelMate.Core.Entities;
using TravelMate.Data;
using TravelMate.Service.Interfaces;
using TravelMate.Interop;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TravelMate.Service.Implementations
{
    // 天气服务实现
    public class WeatherService : IWeatherService
    {
        private readonly ILogger<WeatherService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "d956654a9b90181094c0636f7888f326";

        public WeatherService(ILogger<WeatherService> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<Weather> GetWeatherAsync(string location, DateTime date)
        {
            try
            {
                _logger.LogInformation($"Getting weather for {location} on {date:yyyy-MM-dd}");

                // 构建高德地图API请求URL
                var url = $"https://restapi.amap.com/v3/weather/weatherInfo?key={_apiKey}&city={location}&extensions=all&output=JSON";

                _logger.LogInformation($"Requesting URL: {url}");

                // 发起HTTP请求
                var response = await _httpClient.GetStringAsync(url);

                _logger.LogInformation($"API Response: {response.Substring(0, Math.Min(500, response.Length))}...");

                // 解析JSON响应
                var weatherData = ParseWeatherResponse(response, date, location);

                _logger.LogInformation($"Successfully retrieved weather data for {location}");
                return weatherData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting weather for {location} on {date:yyyy-MM-dd}");
                throw new Exception($"Error getting weather information: {ex.Message}", ex);
            }
        }

        private Weather ParseWeatherResponse(string jsonResponse, DateTime date, string location)
        {
            try
            {
                using var document = JsonDocument.Parse(jsonResponse);
                var root = document.RootElement;

                // 检查API状态
                if (!root.TryGetProperty("status", out var statusElement) || statusElement.GetString() != "1")
                {
                    var errorInfo = root.TryGetProperty("info", out var info) ? info.GetString() : "Unknown error";
                    throw new Exception($"API returned error: {errorInfo}");
                }

                // 获取预报数据
                if (!root.TryGetProperty("forecasts", out var forecasts) || forecasts.GetArrayLength() == 0)
                {
                    throw new Exception("No weather data available for the location");
                }

                var firstForecast = forecasts[0];
                if (!firstForecast.TryGetProperty("casts", out var casts) || casts.GetArrayLength() == 0)
                {
                    throw new Exception("No detailed forecast data available");
                }

                var targetDate = date.ToString("yyyy-MM-dd");

                // 查找指定日期的天气数据
                foreach (var cast in casts.EnumerateArray())
                {
                    if (cast.TryGetProperty("date", out var dateElement) &&
                        dateElement.GetString() == targetDate)
                    {
                        return new Weather
                        {
                            MaxTemperature = ParseFloat(cast, "daytemp"),
                            MinTemperature = ParseFloat(cast, "nighttemp"),
                            Description = ParseString(cast, "dayweather"),
                            Wind = ParseString(cast, "daypower"),
                            Date = date,
                            Location = location
                        };
                    }
                }

                // 如果找不到指定日期，返回第一个可用的预报数据
                var firstCast = casts[0];
                return new Weather
                {
                    MaxTemperature = ParseFloat(firstCast, "daytemp"),
                    MinTemperature = ParseFloat(firstCast, "nighttemp"),
                    Description = ParseString(firstCast, "dayweather"),
                    Wind = ParseString(firstCast, "daypower"),
                    Date = date,
                    Location = location
                };
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to parse weather API response: {ex.Message}", ex);
            }
        }

        private static float ParseFloat(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var prop))
            {
                if (prop.ValueKind == JsonValueKind.String)
                {
                    return float.TryParse(prop.GetString(), out var result) ? result : 0f;
                }
                else if (prop.ValueKind == JsonValueKind.Number)
                {
                    return prop.GetSingle();
                }
            }
            return 0f;
        }

        private static string ParseString(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var prop))
            {
                return prop.GetString() ?? "";
            }
            return "";
        }
    }
}