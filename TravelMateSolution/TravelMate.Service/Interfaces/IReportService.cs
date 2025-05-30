using System.Collections.Generic;
using TravelMate.Core.Entities;

namespace TravelMate.Service.Interfaces
{
	public interface IReportService
	{
		ItineraryReport GetAllBudgetAndExpense(int userId);
		ItineraryReport GetEventBudgetAndExpense(List<int> itineraryIds);
	}
}
