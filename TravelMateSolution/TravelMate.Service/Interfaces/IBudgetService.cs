using System.Collections.Generic;
using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;

namespace TravelMate.Service.Interfaces
{
	public interface IBudgetService
	{
		int AddBudget(Budget budget);
		int ModifyBudget(BudgetUpdateDTO dto);
		List<Budget> GetEventBudget(int eveId);
		bool DeleteBudgetByEveId(int eveId);
		bool RemoveById(int id);
	}
}
