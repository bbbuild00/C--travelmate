using Microsoft.AspNetCore.Mvc;
using TravelMate.Core.Entities;
using TravelMate.Core.Response;
using TravelMate.Service.Interfaces;

namespace TravelMate.API.Controllers
{
	[ApiController]
	[Route("user")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}


		[HttpGet("info")]
		public IActionResult GetUserInfo(int userID)
		{
			var user = _userService.GetUserById(userID);
			return Ok(Result.Success(user));
		}
	}
}
