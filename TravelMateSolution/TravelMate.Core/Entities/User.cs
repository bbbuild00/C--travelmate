using System.ComponentModel.DataAnnotations.Schema;

namespace TravelMate.Core.Entities
{
    [Table("user")]
    public class User
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public string Name { get; set; }
        public int? Gender { get; set; }  // 确保是可空类型
    }
}