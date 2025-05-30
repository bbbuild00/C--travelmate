using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;
using TravelMate.Data;
using TravelMate.Service.Interfaces;

namespace TravelMate.Service.Implementations
{
	public class ExpenseService : IExpenseService
	{
		private readonly TravelMateDbContext _dbContext;
		//private readonly IItineraryService _itineraryService;

		//public ExpenseService(TravelMateDbContext dbContext, IItineraryService itineraryService)
		//{
		//	_dbContext = dbContext;
		//	_itineraryService = itineraryService;
		//}

		public ExpenseService(TravelMateDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public int AddExpense(Expense expense)
		{
			_dbContext.Expenses.Add(expense);
			var result = _dbContext.SaveChanges();
			return result > 0 ? expense.Id : 0;
		}

		public int CountRecordsByEveId(int eveId)
		{
			return _dbContext.Expenses.Count(e => e.EveId == eveId);
		}

		public int ModifyExpense(ExpenseUpdateDTO dto)
		{
			var existing = _dbContext.Expenses.FirstOrDefault(e => e.Id == dto.Id);
			if (existing == null) return 0;

			if (dto.EveId.HasValue) existing.EveId = dto.EveId.Value;
			if (dto.Type.HasValue) existing.Type = dto.Type.Value;
			if (dto.Time.HasValue) existing.Time = dto.Time.Value.Date; // 精确到天
			if (dto.Money.HasValue) existing.Money = dto.Money.Value;
			if (!string.IsNullOrEmpty(dto.Name)) existing.Name = dto.Name;

			_dbContext.Expenses.Update(existing);
			_dbContext.SaveChanges();
			return existing.Id;
		}


		public List<Expense> GetEventExpense(int eveId)
		{
			return _dbContext.Expenses
				.Where(e => e.EveId == eveId)
				.ToList();
		}

		public TimeReport GetTimeExpense(string startTime, string endTime, int userId)
		{
			//var eveIds = _itineraryService.GetEventIdsByUserId(userId);

			// 直接查找用户的 event ID 列表
			var eveIds = _dbContext.Itineraries
				.Where(i => i.UserId == userId)
				.SelectMany(i => _dbContext.Events
					.Where(e => e.ItiId == i.Id)
					.Select(e => e.Id))
				.ToList();

			if (eveIds == null || !eveIds.Any())
				return new TimeReport(new List<Expense>(), 0);

			var start = DateTime.Parse(startTime);
			var end = DateTime.Parse(endTime);

			var expenses = _dbContext.Expenses
				.Where(e => eveIds.Contains(e.EveId) && e.Time >= start && e.Time <= end)
				.ToList();

			var total = expenses.Sum(e => (int)Math.Round(e.Money));
			return new TimeReport(expenses, total);
		}

		public bool DeleteExpenseByEveId(int eveId)
		{
			var toRemove = _dbContext.Expenses
				.Where(e => e.EveId == eveId)
				.ToList();

			_dbContext.Expenses.RemoveRange(toRemove);
			_dbContext.SaveChanges();
			return true;
		}

		public TimeReport GetAllExpense(int userId)
		{
			//var eveIds = _itineraryService.GetEventIdsByUserId(userId);
			// 直接查找用户的 event ID 列表
			var eveIds = _dbContext.Itineraries
				.Where(i => i.UserId == userId)
				.SelectMany(i => _dbContext.Events
					.Where(e => e.ItiId == i.Id)
					.Select(e => e.Id))
				.ToList();

			if (eveIds == null || !eveIds.Any())
				return new TimeReport(new List<Expense>(), 0);

			var expenses = _dbContext.Expenses
				.Where(e => eveIds.Contains(e.EveId))
				.ToList();

			var total = expenses.Sum(e => (int)Math.Round(e.Money));
			return new TimeReport(expenses, total);
		}

		public TypeReport GetTypeExpense(List<int> types, int userId)
		{
			//var eveIds = _itineraryService.GetEventIdsByUserId(userId);
			// 直接查找用户的 event ID 列表
			var eveIds = _dbContext.Itineraries
				.Where(i => i.UserId == userId)
				.SelectMany(i => _dbContext.Events
					.Where(e => e.ItiId == i.Id)
					.Select(e => e.Id))
				.ToList();
			if (eveIds == null || !eveIds.Any())
				return new TypeReport(new List<Expense>(), 0);

			var expenses = _dbContext.Expenses
				.Where(e => eveIds.Contains(e.EveId) && types.Contains(e.Type))
				.ToList();

			var total = expenses.Sum(e => (int)Math.Round(e.Money));
			return new TypeReport(expenses, total);
		}
		public bool RemoveById(int id)
		{
			var expense = _dbContext.Expenses.FirstOrDefault(e => e.Id == id);
			if (expense == null) return false;

			_dbContext.Expenses.Remove(expense);
			_dbContext.SaveChanges();
			return true;
		}

	}
}
