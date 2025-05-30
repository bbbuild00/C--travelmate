using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravelMate.Core.Entities;
using TravelMate.Core.Response;
using TravelMate.Core.ViewModels;
using TravelMate.Service.Interfaces;

namespace TravelMate.API.Controllers
{
	[ApiController]
	[Route("itinerary")]
	public class ItineraryController : ControllerBase
	{
		private readonly IItineraryService _itineraryService;
		private readonly ILogger<ItineraryController> _logger;

		public ItineraryController(IItineraryService itineraryService, ILogger<ItineraryController> logger)
		{
			_itineraryService = itineraryService;
			_logger = logger;
		}

		[HttpPost("add")]
		public IActionResult AddItinerary([FromBody] Itinerary itinerary)
		{
			_logger.LogInformation("收到新增请求：{Name}", itinerary.Name);
			var id = _itineraryService.AddItinerary(itinerary);
			return Ok(Result.Success(id));
		}

		[HttpGet("getall")]
		public IActionResult GetAllItinerary([FromQuery] int userId)
		{
			var itineraries = _itineraryService.GetItinerariesByUserId(userId);
			return Ok(Result.Success(itineraries));
		}

		[HttpGet("getone")]
		public IActionResult GetOneItinerary([FromQuery] int id)
		{
			var vo = _itineraryService.GetItineraryById(id);
			return Ok(Result.Success(vo));
		}

		[HttpPut("modify")]
		public IActionResult ModifyItinerary([FromBody] ItineraryUpdateVo vo)
		{
			var id = _itineraryService.ModifyItinerary(vo);
			return Ok(Result.Success(id));
		}


		[HttpDelete("delete")]
		public IActionResult DeleteItinerary([FromQuery] int id)
		{
			var result = _itineraryService.DeleteItinerary(id);
			return Ok(Result.Success(result));
		}

		[HttpGet("eventids")]
		public IActionResult GetAllEvent([FromQuery] int userId)
		{
			var ids = _itineraryService.GetEventIdsByUserId(userId);
			return Ok(Result.Success(ids));
		}
	}
}
