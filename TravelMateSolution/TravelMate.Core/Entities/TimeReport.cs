using TravelMate.Core.Entities;

public class TimeReport
{
	public List<Expense> Expenses { get; set; }
	public int TotalExpense { get; set; }

	public TimeReport() { }

	public TimeReport(List<Expense> expenses, int totalExpense)
	{
		Expenses = expenses;
		TotalExpense = totalExpense;
	}
}
