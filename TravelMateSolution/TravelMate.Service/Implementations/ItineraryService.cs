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
	public class ItineraryService : IItineraryService
	{
		private readonly TravelMateDbContext _context;
		private readonly ILogger<ItineraryService> _logger;
		private readonly IBudgetService _budgetService;
		private readonly IExpenseService _expenseService;

		//private IEventService? _eventService; // ✅ 改为可选的延迟注入字段

		public ItineraryService(
		TravelMateDbContext context,
		ILogger<ItineraryService> logger,
		IBudgetService budgetService,
		IExpenseService expenseService)
		{
			_context = context;
			_logger = logger;
			_budgetService = budgetService;
			_expenseService = expenseService;
		}


		public int AddItinerary(Itinerary itinerary)
		{
			_context.Itineraries.Add(itinerary);
			var result = _context.SaveChanges();
			return result > 0 ? itinerary.Id : 0;
		}

		public List<Itinerary> GetItinerariesByUserId(int userId)
		{
			return _context.Itineraries.Where(i => i.UserId == userId).ToList();
		}

		//public ItineraryVo GetItineraryById(int id)
		//{
		//	var itinerary = _context.Itineraries.FirstOrDefault(i => i.Id == id);
		//	if (itinerary == null ) return null;

		//	return new ItineraryVo
		//	{
		//		Id = itinerary.Id,
		//		UserId = itinerary.UserId,
		//		Name = itinerary.Name,
		//		StartDate = itinerary.StartDate,
		//		EndDate = itinerary.EndDate,
		//		Location = itinerary.Location,
		//		Events = _eventService.FindByItiID(id)
		//	};

		public ItineraryVo GetItineraryById(int id)
		{
			var itinerary = _context.Itineraries.FirstOrDefault(i => i.Id == id);
			if (itinerary == null) return null;

			// 查找对应的所有 Event
			var events = _context.Events.Where(e => e.ItiId == id).ToList();
			var eventVos = new List<EventVo>();

			foreach (var evt in events)
			{
				var budgets = _context.Budgets
					.Where(b => b.EveId == evt.Id)
					.ToList();

				var expenses = _context.Expenses
					.Where(ex => ex.EveId == evt.Id)
					.ToList();

				var eventVo = new EventVo
				{
					Id = evt.Id,
					ItiId = evt.ItiId,
					StartTime = evt.StartTime,
					EndTime = evt.EndTime,
					Location = evt.Location,
					Description = evt.Description,
					Name = evt.Name,
					Type = evt.Type,
					Budgets = budgets,
					Expenses = expenses
				};

				eventVos.Add(eventVo);
			}

			return new ItineraryVo
			{
				Id = itinerary.Id,
				UserId = itinerary.UserId,
				Name = itinerary.Name,
				StartDate = itinerary.StartDate,
				EndDate = itinerary.EndDate,
				Location = itinerary.Location,
				Events = eventVos
			};
		}


		public int ModifyItinerary(ItineraryUpdateVo vo)
		{
			var entity = _context.Itineraries.FirstOrDefault(i => i.Id == vo.Id);
			if (entity == null) return 0;

			if (vo.UserId.HasValue) entity.UserId = vo.UserId.Value;
			if (!string.IsNullOrEmpty(vo.Name)) entity.Name = vo.Name;
			if (vo.StartDate.HasValue) entity.StartDate = vo.StartDate.Value.Date;
			if (vo.EndDate.HasValue) entity.EndDate = vo.EndDate.Value.Date;
			if (!string.IsNullOrEmpty(vo.Location)) entity.Location = vo.Location;

			var result = _context.SaveChanges();
			_logger.LogInformation("行程ID为{Id}的记录更新{Result}", vo.Id, result > 0 ? "成功" : "失败");
			return result > 0 ? entity.Id : 0;
		}


		//public bool DeleteItinerary(int id)
		//{

		//	var events = _eventService.FindByItiID(id);
		//	foreach (var ev in events)
		//	{
		//		var deleted = _eventService.DeleteEvent(ev.Id);
		//		if (!deleted)
		//		{
		//			_logger.LogWarning("删除事件ID为{Id}时失败", ev.Id);
		//			return false;
		//		}
		//	}

		//	var itinerary = _context.Itineraries.Find(id);
		//	if (itinerary == null) return false;

		//	_context.Itineraries.Remove(itinerary);
		//	var result = _context.SaveChanges();
		//	_logger.LogInformation("行程ID为{Id}及其所有事件删除{Result}", id, result > 0 ? "成功" : "失败");
		//	return result > 0;
		//}
		public bool DeleteItinerary(int id)
		{
			// 直接查找事件
			var events = _context.Events.Where(e => e.ItiId == id).ToList();
			foreach (var ev in events)
			{
				var evt = _context.Events.FirstOrDefault(e => e.Id == ev.Id);
				if (evt == null)
				{
					_logger.LogWarning("删除事件 ID 为 {Id} 时失败", ev.Id);
					return false;
				}

				_budgetService.DeleteBudgetByEveId(ev.Id);
				_expenseService.DeleteExpenseByEveId(ev.Id);
				_context.Events.Remove(evt);
			}

			// 删除行程
			var itinerary = _context.Itineraries.Find(id);
			if (itinerary == null) return false;

			_context.Itineraries.Remove(itinerary);
			var result = _context.SaveChanges();
			_logger.LogInformation("行程 ID 为 {Id} 及所有事件删除 {Result}", id, result > 0 ? "成功" : "失败");

			return result > 0;
		}

		public ItineraryDTO GetItineraryDetails(int id)
		{
			var itinerary = _context.Itineraries.Find(id);
			if (itinerary == null)
				throw new Exception("Itinerary not found for ID: " + id);

			return new ItineraryDTO
			{
				StartDate = itinerary.StartDate,
				EndDate = itinerary.EndDate,
				Location = itinerary.Location
			};
		}

		//public List<int> GetEventIdsByUserId(int userId)
		//{

		//	var itineraries = _context.Itineraries.Where(i => i.UserId == userId).ToList();
		//	var eventIds = new List<int>();

		//	foreach (var itinerary in itineraries)
		//	{
		//		var events = _eventService.FindByItiID(itinerary.Id);
		//		eventIds.AddRange(events.Select(e => e.Id));
		//	}

		//	return eventIds;
		//}
		public List<int> GetEventIdsByUserId(int userId)
		{
			var itineraries = _context.Itineraries
				.Where(i => i.UserId == userId)
				.ToList();

			var eventIds = new List<int>();

			foreach (var itinerary in itineraries)
			{
				var events = _context.Events
					.Where(e => e.ItiId == itinerary.Id)
					.ToList();

				eventIds.AddRange(events.Select(e => e.Id));
			}

			return eventIds;
		}

	}
}
