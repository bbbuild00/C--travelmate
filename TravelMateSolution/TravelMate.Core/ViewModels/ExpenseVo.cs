using System;

namespace TravelMate.Core.ViewModels
{
	public class ExpenseVo
	{
		public int Id { get; set; }
		public int EveId { get; set; }
		public int Type { get; set; }
		public DateTime Time { get; set; }
		public float Money { get; set; }
		public string Name { get; set; }
	}
}
