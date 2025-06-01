using System.Text.Json.Serialization;

namespace TravelMate.Core.ViewModels
{
    // 天气视图模型
    public class WeatherVo
    {
        [JsonPropertyName("maxTemperature")]
        public float MaxTemperature { get; set; }

        [JsonPropertyName("minTemperature")]
        public float MinTemperature { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("wind")]
        public string Wind { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;
    }
}
