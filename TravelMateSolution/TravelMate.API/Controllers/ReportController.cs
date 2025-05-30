using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TravelMate.Core.Entities;
using TravelMate.Core.Response;
using TravelMate.Service.Interfaces;

namespace TravelMate.API.Controllers
{
	[ApiController]
	[Route("report")]
	public class ReportController : ControllerBase
	{
		private readonly IExpenseService _expenseService;
		private readonly IReportService _reportService;

		public ReportController(IExpenseService expenseService, IReportService reportService)
		{
			_expenseService = expenseService;
			_reportService = reportService;
		}

		[HttpGet("getall")]
		public IActionResult GetAllExpense([FromQuery] int userId)
		{
			var report = _expenseService.GetAllExpense(userId);
			return Ok(Result.Success(report));
		}

		[HttpGet("type")]
		public IActionResult GetTypeExpense([FromQuery] string types, [FromQuery] int userId)
		{
			var typeList = types.Split(',').Select(int.Parse).ToList();
			var report = _expenseService.GetTypeExpense(typeList, userId);
			return Ok(Result.Success(report));
		}


		[HttpGet("time")]
		public IActionResult GetTimeExpense([FromQuery] string startTime, [FromQuery] string endTime, [FromQuery] int userId)
		{
			var report = _expenseService.GetTimeExpense(startTime, endTime, userId);
			return Ok(Result.Success(report));
		}

		[HttpGet("budgetandexpense")]
		public IActionResult GetAllBudgetAndExpense([FromQuery] int userId)
		{
			var report = _reportService.GetAllBudgetAndExpense(userId);
			return Ok(Result.Success(report));
		}

		[HttpGet("iti")]
		public IActionResult GetItiIDsBudgetAndExpense([FromQuery] string itiIDs)
		{
			var ids = itiIDs.Split(',').Select(id => int.Parse(id)).ToList();
			var report = _reportService.GetEventBudgetAndExpense(ids);
			return Ok(Result.Success(report));
		}

	}
}
