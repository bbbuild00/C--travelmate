
using System.Text.Json.Serialization;

namespace TravelMate.Core.DTOs
{
    // 天气查询请求 DTO
    public class WeatherRequestDTO
    {
        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}
