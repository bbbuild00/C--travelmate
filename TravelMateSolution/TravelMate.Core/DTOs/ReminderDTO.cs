using System.Text.Json.Serialization;

namespace TravelMate.Core.DTOs
{
    public class ReminderDTO
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
    }
}
