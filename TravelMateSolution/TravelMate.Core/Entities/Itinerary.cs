using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TravelMate.Core.Entities
{
	[Table("itinerary")]
	public class Itinerary
	{
		[Key]
		[Column("ID")]
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[Column("user_id")]
		[JsonPropertyName("userID")]
		public int UserId { get; set; }

		[Column("name")]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		private DateTime _startDate;

		[Column("start_date")]
		[JsonPropertyName("startDate")]
		public DateTime StartDate
		{
			get => _startDate;
			set => _startDate = value.Date;  // 自动截断时间部分
		}

		private DateTime _endDate;

		[Column("end_date")]
		[JsonPropertyName("endDate")]
		public DateTime EndDate
		{
			get => _endDate;
			set => _endDate = value.Date;  // 自动截断时间部分
		}

		[Column("location")]
		[JsonPropertyName("location")]
		public string Location { get; set; }
	}
}
