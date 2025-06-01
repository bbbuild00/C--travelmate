using Microsoft.AspNetCore.Mvc;
using TravelMate.Core.Entities;
using TravelMate.Core.Response;
using TravelMate.Service.Interfaces;

namespace TravelMate.API.Controllers
{
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        private readonly IWeatherService _weatherService;

        public WeatherController(IReminderService reminderService, IWeatherService weatherService)
        {
            _reminderService = reminderService;
            _weatherService = weatherService;
        }

        // 完全对应 Java: @GetMapping("/weather/get")
        [HttpGet("/weather/get")]
        public async Task<IActionResult> GetWeather([FromQuery] string location, [FromQuery] DateTime date)
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(location, date);
                return Ok(Result.Success(weather));
            }
            catch (Exception ex)
            {
                return Ok(new Result
                {
                    Code = 0,
                    Msg = ex.Message,
                    Data = null
                });
            }
        }

        // 完全对应 Java: @PostMapping("/reminder/add")
        [HttpPost("/reminder/add")]
        public async Task<IActionResult> AddReminder([FromBody] Reminder reminder)
        {
            try
            {
                var id = await _reminderService.AddReminderAsync(reminder);
                return Ok(Result.Success(id));
            }
            catch (Exception ex)
            {
                return Ok(new Result
                {
                    Code = 0,
                    Msg = ex.Message,
                    Data = null
                });
            }
        }

        // 完全对应 Java: @PutMapping("/reminder/modify")
        [HttpPut("/reminder/modify")]
        public async Task<IActionResult> ModifyReminder([FromBody] Reminder reminder)
        {
            try
            {
                var id = await _reminderService.ModifyReminderAsync(reminder);
                return Ok(Result.Success(id));
            }
            catch (Exception ex)
            {
                return Ok(new Result
                {
                    Code = 0,
                    Msg = ex.Message,
                    Data = null
                });
            }
        }

        // 完全对应 Java: @DeleteMapping("/reminder/delete")
        [HttpDelete("/reminder/delete")]
        public async Task<IActionResult> DeleteReminder([FromQuery] int id)
        {
            try
            {
                var result = await _reminderService.DeleteReminderAsync(id);
                return Ok(Result.Success(result));
            }
            catch (Exception ex)
            {
                return Ok(new Result
                {
                    Code = 0,
                    Msg = ex.Message,
                    Data = null
                });
            }
        }

        // 完全对应 Java: @GetMapping("/reminder/get")
        [HttpGet("/reminder/get")]
        public async Task<IActionResult> GetReminder([FromQuery] int userID)
        {
            try
            {
                var reminders = await _reminderService.GetRemindersByUserIdAsync(userID);
                return Ok(Result.Success(reminders));
            }
            catch (Exception ex)
            {
                return Ok(new Result
                {
                    Code = 0,
                    Msg = ex.Message,
                    Data = null
                });
            }
        }
    }
}