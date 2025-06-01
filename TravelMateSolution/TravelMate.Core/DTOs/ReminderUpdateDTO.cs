using System.Text.Json.Serialization;

namespace TravelMate.Core.DTOs
{
    // 提醒更新 DTO
    public class ReminderUpdateDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("time")]
        public DateTime? Time { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("userID")]
        public int? UserId { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
    }
}
