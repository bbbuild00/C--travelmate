using System;
using System.Collections.Generic;
using System.Linq;
using TravelMate.Core.Entities;
using TravelMate.Service.Interfaces;

namespace TravelMate.Service.Implementations
{
	public class ReportService : IReportService
	{
		private readonly IItineraryService _itineraryService;
		private readonly IExpenseService _expenseService;
		private readonly IBudgetService _budgetService;
		private readonly IEventService _eventService;

		public ReportService(
		IItineraryService itineraryService,
		IExpenseService expenseService,
		IBudgetService budgetService,
		IEventService eventService) // ← 添加这一行
			{
				_itineraryService = itineraryService;
				_expenseService = expenseService;
				_budgetService = budgetService;
				_eventService = eventService; // ← 赋值这一行
			}


		public ItineraryReport GetAllBudgetAndExpense(int userId)
		{
			var eveIds = _itineraryService.GetEventIdsByUserId(userId);
			if (eveIds == null || !eveIds.Any())
			{
				return new ItineraryReport(new List<Expense>(), new List<Budget>(), 0, 0);
			}

			var allExpenses = new List<Expense>();
			var allBudgets = new List<Budget>();
			int totalExpense = 0;
			int totalBudget = 0;

			foreach (var eveId in eveIds)
			{
				var expenses = _expenseService.GetEventExpense(eveId);
				allExpenses.AddRange(expenses);
				totalExpense += expenses.Sum(e => (int)Math.Round(e.Money));

				var budgets = _budgetService.GetEventBudget(eveId);
				allBudgets.AddRange(budgets);
				totalBudget += budgets.Sum(b => (int)Math.Round(b.Money));
			}

			return new ItineraryReport(allExpenses, allBudgets, totalExpense, totalBudget);
		}

		public ItineraryReport GetEventBudgetAndExpense(List<int> itineraryIds)
		{
			var eveIds = _eventService.GetEventByItiIDs(itineraryIds);
			if (eveIds == null || !eveIds.Any())
			{
				return new ItineraryReport(new List<Expense>(), new List<Budget>(), 0, 0);
			}

			var allExpenses = new List<Expense>();
			var allBudgets = new List<Budget>();
			int totalExpense = 0;
			int totalBudget = 0;

			foreach (var eveId in eveIds)
			{
				var expenses = _expenseService.GetEventExpense(eveId);
				allExpenses.AddRange(expenses);
				totalExpense += expenses.Sum(e => (int)Math.Round(e.Money));

				var budgets = _budgetService.GetEventBudget(eveId);
				allBudgets.AddRange(budgets);
				totalBudget += budgets.Sum(b => (int)Math.Round(b.Money));
			}

			return new ItineraryReport(allExpenses, allBudgets, totalExpense, totalBudget);
		}
	}
}
