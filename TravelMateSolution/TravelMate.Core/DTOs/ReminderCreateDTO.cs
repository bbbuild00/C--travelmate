using System.Text.Json.Serialization;

namespace TravelMate.Core.DTOs
{
    // 提醒创建 DTO
    public class ReminderCreateDTO
    {
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("userID")]
        public int UserId { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}
