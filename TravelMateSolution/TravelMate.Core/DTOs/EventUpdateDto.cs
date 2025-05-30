using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TravelMate.Core.DTOs
{
	public class EventUpdateDto
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("itiID")]
		public int? ItiId { get; set; }

		[JsonPropertyName("startTime")]
		public DateTime? StartTime { get; set; }

		[JsonPropertyName("endTime")]
		public DateTime? EndTime { get; set; }

		[JsonPropertyName("location")]
		public string? Location { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("type")]
		public int? Type { get; set; }
	}

}
