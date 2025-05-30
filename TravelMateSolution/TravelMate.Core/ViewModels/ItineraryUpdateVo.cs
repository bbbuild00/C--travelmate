using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TravelMate.Core.ViewModels
{
	public class ItineraryUpdateVo
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("userID")]
		public int? UserId { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("startDate")]
		public DateTime? StartDate { get; set; }

		[JsonPropertyName("endDate")]
		public DateTime? EndDate { get; set; }

		[JsonPropertyName("location")]
		public string? Location { get; set; }
	}

}
