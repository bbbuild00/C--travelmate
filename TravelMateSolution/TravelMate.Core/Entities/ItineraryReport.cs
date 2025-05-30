using System.Collections.Generic;

namespace TravelMate.Core.Entities
{
	public class ItineraryReport
	{
		public List<Expense> Expenses { get; set; }
		public List<Budget> Budgets { get; set; }
		public int TotalExpense { get; set; }
		public int TotalBudget { get; set; }

		// 添加构造函数以解决 CS1729 报错
		public ItineraryReport(List<Expense> expenses, List<Budget> budgets, int totalExpense, int totalBudget)
		{
			Expenses = expenses;
			Budgets = budgets;
			TotalExpense = totalExpense;
			TotalBudget = totalBudget;
		}
	}
}
