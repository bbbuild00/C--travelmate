using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;
using TravelMate.Core.Response;
using TravelMate.Service.Interfaces;

namespace TravelMate.API.Controllers
{
	[ApiController]
	[Route("budget")]
	public class BudgetController : ControllerBase
	{
		private readonly IBudgetService _budgetService;
		private readonly ILogger<BudgetController> _logger;

		public BudgetController(IBudgetService budgetService, ILogger<BudgetController> logger)
		{
			_budgetService = budgetService;
			_logger = logger;
		}

		[HttpPost("add")]
		public IActionResult AddBudget([FromBody] Budget budget)
		{
			_logger.LogInformation("add budget");
			var id = _budgetService.AddBudget(budget);
			return Ok(Result.Success(id));
		}

		[HttpPut("modify")]
		public IActionResult ModifyBudget([FromBody] BudgetUpdateDTO budgetDto)
		{
			var id = _budgetService.ModifyBudget(budgetDto);
			return Ok(Result.Success(id));
		}


		[HttpDelete("delete")]
		public IActionResult DeleteBudget([FromQuery] int id)
		{
			var result = _budgetService.RemoveById(id);
			return Ok(Result.Success(result));
		}

		[HttpGet("event")]
		public IActionResult GetEventBudget([FromQuery] int eveId)
		{
			var budgets = _budgetService.GetEventBudget(eveId);
			return Ok(Result.Success(budgets));
		}

		[HttpDelete("deletebyeveid")]
		public IActionResult DeleteBudgetByEveId([FromQuery] int eveId)
		{
			var result = _budgetService.DeleteBudgetByEveId(eveId);
			return Ok(Result.Success(result));
		}
	}
}
