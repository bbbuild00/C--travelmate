using System;

namespace TravelMate.Core.DTOs
{
	public class ItineraryDTO
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Location { get; set; }
	}
}
