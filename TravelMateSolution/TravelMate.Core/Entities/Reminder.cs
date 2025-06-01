using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TravelMate.Core.Entities
{
    // 提醒实体类
    [Table("reminder")]
    public class Reminder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("time")]
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [Column("location")]
        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [Column("userid")]
        [JsonPropertyName("userID")]
        public int UserId { get; set; }

        private DateTime _date;

        [Column("date")]
        [JsonPropertyName("date")]
        public DateTime Date
        {
            get => _date;
            set => _date = value.Date; // 自动截断时间部分
        }
    }
}
