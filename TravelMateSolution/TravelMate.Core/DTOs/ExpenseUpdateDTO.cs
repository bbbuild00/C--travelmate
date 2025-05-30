using System;
using System.Text.Json.Serialization;

namespace TravelMate.Core.DTOs
{
	public class ExpenseUpdateDTO
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("eveID")]
		public int? EveId { get; set; }

		[JsonPropertyName("type")]
		public int? Type { get; set; }

		[JsonPropertyName("time")]
		public DateTime? Time { get; set; }

		[JsonPropertyName("money")]
		public float? Money { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }
	}
}
