using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TravelMate.Core.Entities;

namespace TravelMate.Core.ViewModels
{
	public class EventVo
	{
		public int Id { get; set; }
		[JsonPropertyName("itiID")]
		public int ItiId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public int Type { get; set; }
		public List<Expense> Expenses { get; set; }
		public List<Budget> Budgets { get; set; }
	}
}
