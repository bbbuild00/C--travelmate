using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TravelMate.Core.Entities
{
	[Table("event")]
	public class Event
	{
		[Key]
		[Column("ID")]
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[Column("iti_id")]
		[JsonPropertyName("itiID")]
		public int ItiId { get; set; }

		[Column("start_time")]
		[JsonPropertyName("startTime")]
		public DateTime StartTime { get; set; }

		[Column("end_time")]
		[JsonPropertyName("endTime")]
		public DateTime EndTime { get; set; }

		[Column("location")]
		[JsonPropertyName("location")]
		public string Location { get; set; }

		[Column("description")]
		[JsonPropertyName("description")]
		public string Description { get; set; }

		[Column("name")]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[Column("type")]
		[JsonPropertyName("type")]
		public int Type { get; set; }
	}
}
