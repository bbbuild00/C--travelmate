using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelMate.Core.ViewModels
{
	public class ItineraryVo
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("userID")]
		public int UserId { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Location { get; set; }
		public List<EventVo> Events { get; set; }
	}
}
