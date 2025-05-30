using System;
using System.Collections.Generic;
using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;

namespace TravelMate.Service.Interfaces
{
	public interface IExpenseService
	{
		int AddExpense(Expense expense);
		int CountRecordsByEveId(int eveId);
		int ModifyExpense(ExpenseUpdateDTO dto);
		List<Expense> GetEventExpense(int eveId);
		TimeReport GetTimeExpense(string startTime, string endTime, int userId);
		bool DeleteExpenseByEveId(int eveId);
		TimeReport GetAllExpense(int userId);
		TypeReport GetTypeExpense(List<int> types, int userId);
		bool RemoveById(int id); 

	}
}
