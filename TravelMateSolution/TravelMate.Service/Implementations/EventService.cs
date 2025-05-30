using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;
using TravelMate.Core.ViewModels;
using TravelMate.Data;
using TravelMate.Service.Interfaces;

namespace TravelMate.Service.Implementations
{
	public class EventService : IEventService
	{
		private readonly TravelMateDbContext _context;
		private readonly ILogger<EventService> _logger;
		private readonly IBudgetService _budgetService;
		private readonly IExpenseService _expenseService;

		public EventService(TravelMateDbContext context, ILogger<EventService> logger, IBudgetService budgetService, IExpenseService expenseService)
		{
			_context = context;
			_logger = logger;
			_budgetService = budgetService;
			_expenseService = expenseService;
		}

		public int? AddEvent(Event evt)
		{
			_context.Events.Add(evt);
			var result = _context.SaveChanges();
			return result > 0 ? evt.Id : (int?)null;
		}

		public List<EventVo> FindByItiID(int itiID)
		{
			_logger.LogInformation("查询行程ID为{itiID}的所有事件", itiID);
			var events = _context.Events.Where(e => e.ItiId == itiID).ToList();
			var eventVos = new List<EventVo>();

			foreach (var evt in events)
			{
				var vo = new EventVo
				{
					Id = evt.Id,
					ItiId = evt.ItiId,
					StartTime = evt.StartTime,
					EndTime = evt.EndTime,
					Location = evt.Location,
					Description = evt.Description,
					Name = evt.Name,
					Type = evt.Type,
					Budgets = _budgetService.GetEventBudget(evt.Id),
					Expenses = _expenseService.GetEventExpense(evt.Id)
				};

				eventVos.Add(vo);
			}

			return eventVos;
		}

		public int? ModifyEvent(EventUpdateDto dto)
		{
			var existing = _context.Events.FirstOrDefault(e => e.Id == dto.Id);
			if (existing == null) return null;

			if (dto.ItiId != null) existing.ItiId = dto.ItiId.Value;
			if (dto.StartTime != null) existing.StartTime = dto.StartTime.Value.Date;
			if (dto.EndTime != null) existing.EndTime = dto.EndTime.Value.Date;
			if (!string.IsNullOrWhiteSpace(dto.Location)) existing.Location = dto.Location;
			if (!string.IsNullOrWhiteSpace(dto.Description)) existing.Description = dto.Description;
			if (!string.IsNullOrWhiteSpace(dto.Name)) existing.Name = dto.Name;
			if (dto.Type != null) existing.Type = dto.Type.Value;

			var result = _context.SaveChanges();
			return result > 0 ? dto.Id : (int?)null;
		}


		public bool DeleteEvent(int id)
		{
			var evt = _context.Events.FirstOrDefault(e => e.Id == id);
			if (evt == null) return false;

			_budgetService.DeleteBudgetByEveId(id);
			_expenseService.DeleteExpenseByEveId(id);

			_context.Events.Remove(evt);
			return _context.SaveChanges() > 0;
		}

		public List<int> GetEventByItiIDs(List<int> itiIDs)
		{
			if (itiIDs == null || !itiIDs.Any()) return new List<int>();

			return _context.Events
				.Where(e => itiIDs.Contains(e.ItiId))
				.Select(e => e.Id)
				.ToList();
		}
	}
}
