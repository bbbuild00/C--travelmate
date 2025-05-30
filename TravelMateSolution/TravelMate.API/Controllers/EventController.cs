using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravelMate.Core.DTOs;
using TravelMate.Core.Entities;
using TravelMate.Core.Response;
using TravelMate.Service.Interfaces;

namespace TravelMate.API.Controllers
{
	[ApiController]
	[Route("event")]
	public class EventController : ControllerBase
	{
		private readonly IEventService _eventService;
		private readonly ILogger<EventController> _logger;

		public EventController(IEventService eventService, ILogger<EventController> logger)
		{
			_eventService = eventService;
			_logger = logger;
		}

		[HttpPost("add")]
		public IActionResult AddEvent([FromBody] Event evt)
		{
			_logger.LogInformation("add event");
			var id = _eventService.AddEvent(evt);
			_logger.LogInformation("id={Id}", id);
			return Ok(Result.Success(id));
		}

		[HttpPut("modify")]
		public IActionResult ModifyEvent([FromBody] EventUpdateDto dto)
		{
			var id = _eventService.ModifyEvent(dto);
			return Ok(Result.Success(id));
		}


		[HttpDelete("delete")]
		public IActionResult DeleteEvent([FromQuery] int id)
		{
			var result = _eventService.DeleteEvent(id);
			return Ok(Result.Success(result));
		}

		[HttpGet("getbyitinerary")]
		public IActionResult GetEventsByItineraryIds([FromQuery] List<int> itiIDs)
		{
			var ids = _eventService.GetEventByItiIDs(itiIDs);
			return Ok(Result.Success(ids));
		}
	}
}
