using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TravelMate.Core.DTOs;
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

        /// <summary>
        /// 用户登录接口 - 返回格式与 Java 版本保持一致
        /// </summary>
        /// <param name="code">微信小程序 code</param>
        /// <returns>用户ID（直接返回数字，与Java版本一致）</returns>
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery][Required] string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest(new Result
                    {
                        Code = 0,
                        Msg = "授权码不能为空"
                    });
                }

                var loginResult = await _userService.LoginAsync(code);

                // 直接返回用户ID，与Java版本保持一致
                return Ok(Result.Success(loginResult.UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(new Result
                {
                    Code = 0,
                    Msg = ex.Message
                });
            }
        }

        /// <summary>
        /// 完善用户信息接口
        /// </summary>
        /// <param name="userUpdateDto">用户信息</param>
        /// <returns>用户ID</returns>
        [HttpPut("info")]
        public async Task<IActionResult> CompleteInfo([FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
                // 模型验证
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);

                    return BadRequest(new Result
                    {
                        Code = 0,
                        Msg = string.Join("; ", errors)
                    });
                }

                var userId = await _userService.UpdateUserAsync(userUpdateDto);
                return Ok(Result.Success(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new Result
                {
                    Code = 0,
                    Msg = ex.Message
                });
            }
        }

        /// <summary>
        /// 获取用户信息接口
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>用户信息</returns>
        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo([FromQuery][Required] int userID)
        {
            try
            {
                if (userID <= 0)
                {
                    return BadRequest(new Result
                    {
                        Code = 0,
                        Msg = "用户ID必须大于0"
                    });
                }

                var user = await _userService.GetUserByIdAsync(userID);
                if (user == null)
                {
                    return NotFound(new Result
                    {
                        Code = 0,
                        Msg = "用户不存在"
                    });
                }

                return Ok(Result.Success(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new Result
                {
                    Code = 0,
                    Msg = ex.Message
                });
            }
        }
    }
}