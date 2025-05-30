using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TravelMate.Core.Entities
{
	[Table("budget")]
	public class Budget
	{
		[Key]
		[Column("id")]
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[Column("eve_id")]
		[JsonPropertyName("eveID")]
		public int EveId { get; set; }

		[Column("money")]
		[JsonPropertyName("money")]
		public float Money { get; set; }
	}
}
