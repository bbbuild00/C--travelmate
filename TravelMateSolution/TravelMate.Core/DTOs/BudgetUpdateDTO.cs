using System;
using System.Text.Json.Serialization;

namespace TravelMate.Core.DTOs
{
	public class BudgetUpdateDTO
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("eveID")]
		public int? EveId { get; set; }

		[JsonPropertyName("money")]
		public float? Money { get; set; }
	}
}
