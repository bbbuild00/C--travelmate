using Microsoft.AspNetCore.Mvc;
using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;
using TravelMate.Core.Response;
using TravelMate.Service.Interfaces;

namespace TravelMate.API.Controllers
{
	[ApiController]
	[Route("expense")]
	public class ExpenseController : ControllerBase
	{
		private readonly IExpenseService _expenseService;

		public ExpenseController(IExpenseService expenseService)
		{
			_expenseService = expenseService;
		}

		[HttpPost("add")]
		public IActionResult AddExpense([FromBody] Expense expense)
		{
			var id = _expenseService.AddExpense(expense);
			return Ok(Result.Success(id));
		}

		[HttpGet("eventID/number")]
		public IActionResult CountRecordsByEveID([FromQuery] int eveID)
		{
			var count = _expenseService.CountRecordsByEveId(eveID);
			return Ok(Result.Success(count));
		}

		[HttpPut("modify")]
		public IActionResult ModifyExpense([FromBody] ExpenseUpdateDTO dto)
		{
			var id = _expenseService.ModifyExpense(dto);
			return Ok(Result.Success(id));
		}


		[HttpDelete("delete")]
		public IActionResult DeleteExpense([FromQuery] int id)
		{
			var result = _expenseService.RemoveById(id);
			return Ok(Result.Success(result));
		}

		[HttpGet("event")]
		public IActionResult GetEventExpense([FromQuery] int eveID)
		{
			var expenses = _expenseService.GetEventExpense(eveID);
			return Ok(Result.Success(expenses));
		}

		[HttpDelete("deletebyeveid")]
		public IActionResult DeleteExpenseByEveID([FromQuery] int eveID)
		{
			var result = _expenseService.DeleteExpenseByEveId(eveID);
			return Ok(Result.Success(result));
		}
	}
}
