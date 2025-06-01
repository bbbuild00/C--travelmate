using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TravelMate.Core.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Column("openID")]
        [JsonPropertyName("openID")]
        public string OpenId { get; set; } = string.Empty;

        [Column("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [Column("gender")]
        [JsonPropertyName("gender")]
        public int? Gender { get; set; }  // 使用 int? 而不是 byte?，与你的原始代码保持一致
    }
}