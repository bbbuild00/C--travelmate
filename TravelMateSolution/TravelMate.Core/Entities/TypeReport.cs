using TravelMate.Core.Entities;

public class TypeReport
{
	public List<Expense> Expenses { get; set; }
	public int TotalExpense { get; set; }

	public TypeReport() { }

	public TypeReport(List<Expense> expenses, int totalExpense)
	{
		Expenses = expenses;
		TotalExpense = totalExpense;
	}
}
