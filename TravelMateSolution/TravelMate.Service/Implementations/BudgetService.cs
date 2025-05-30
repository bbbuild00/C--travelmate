using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;
using TravelMate.Data;
using TravelMate.Service.Interfaces;

namespace TravelMate.Service.Implementations
{
	public class BudgetService : IBudgetService
	{
		private readonly TravelMateDbContext _dbContext;

		public BudgetService(TravelMateDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public int AddBudget(Budget budget)
		{
			_dbContext.Budgets.Add(budget);
			var result = _dbContext.SaveChanges();
			return result > 0 ? budget.Id : 0;
		}

		public int ModifyBudget(BudgetUpdateDTO dto)
		{
			var existing = _dbContext.Budgets.FirstOrDefault(b => b.Id == dto.Id);
			if (existing == null) return 0;

			if (dto.EveId.HasValue) existing.EveId = dto.EveId.Value;
			if (dto.Money.HasValue) existing.Money = dto.Money.Value;

			_dbContext.Budgets.Update(existing);
			_dbContext.SaveChanges();
			return existing.Id;
		}


		public List<Budget> GetEventBudget(int eveId)
		{
			return _dbContext.Budgets
				.Where(b => b.EveId == eveId)
				.ToList();
		}

		public bool DeleteBudgetByEveId(int eveId)
		{
			var budgets = _dbContext.Budgets
				.Where(b => b.EveId == eveId)
				.ToList();

			_dbContext.Budgets.RemoveRange(budgets);
			_dbContext.SaveChanges();

			return true;
		}
		public bool RemoveById(int id)
		{
			var budget = _dbContext.Budgets.FirstOrDefault(b => b.Id == id);
			if (budget == null) return false;

			_dbContext.Budgets.Remove(budget);
			_dbContext.SaveChanges();
			return true;
		}

	}
}
