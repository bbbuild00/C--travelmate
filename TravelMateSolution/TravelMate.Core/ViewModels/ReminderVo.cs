using System.Text.Json.Serialization;

namespace TravelMate.Core.ViewModels
{
    // 提醒视图模型
    public class ReminderVo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("userID")]
        public int UserId { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("weather")]
        public WeatherVo? Weather { get; set; } // 可选的天气信息
    }
}
