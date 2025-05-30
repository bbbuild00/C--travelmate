using Microsoft.AspNetCore.Mvc;
using TravelMate.Service.Interfaces;
using TravelMate.Core.Response;

namespace TravelMate.API.Controllers
{
	[ApiController]
	[Route("itinerary")]
	public class IntRecController : ControllerBase
	{
		private readonly IIntRecService _intRecService;

		public IntRecController(IIntRecService intRecService)
		{
			_intRecService = intRecService;
		}

		[HttpGet("getintrec")]
		public IActionResult GetIntRec(int id)
		{
			var answer = _intRecService.GetIntRec(id);
			return Ok(Result.Success(answer));
		}
	}
}
