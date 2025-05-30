using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TravelMate.Core.Entities
{
	[Table("expense")]
	public class Expense
	{
		[Key]
		[Column("id")]
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[Column("eve_id")]
		[JsonPropertyName("eveID")]
		public int EveId { get; set; }

		[Column("type")]
		[JsonPropertyName("type")]
		public int Type { get; set; }

		[Column("time")]
		[JsonPropertyName("time")]
		public DateTime Time { get; set; }

		[Column("money")]
		[JsonPropertyName("money")]
		public float Money { get; set; }

		[Column("name")]
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}
