using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using TravelMate.Core.DTOs;
using TravelMate.Data;
using TravelMate.Service.Interfaces;
using TravelMate.Service.Utils;

namespace TravelMate.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly TravelMateDbContext _dbContext;
        private readonly ILogger<UserService> _logger;

        public UserService(TravelMateDbContext dbContext, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<UserLoginResponseDto> LoginAsync(string code)
        {
            string openId = null;

            try
            {
                _logger.LogInformation("Login with code: {code}", code);

                // 1. 调用微信 API 获取 openID
                var wechatResponse = await WeChatUtil.GetSessionKeyOrOpenIdAsync(code);

                if (!wechatResponse.Success)
                {
                    _logger.LogError("Failed to get openid: {error}", wechatResponse.ErrorMessage);
                    throw new Exception($"获取微信用户信息失败: {wechatResponse.ErrorMessage}");
                }

                openId = wechatResponse.OpenId;

                if (string.IsNullOrEmpty(openId))
                {
                    _logger.LogError("OpenId is null or empty");
                    throw new Exception("获取 openid 失败");
                }

                _logger.LogInformation("Got OpenID: {openId}", openId);
            }
            catch (Exception ex) when (ex.Message.Contains("获取微信用户信息失败"))
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling WeChat API");
                throw new Exception("调用微信接口失败");
            }

            try
            {
                // 2. 直接使用原生 SQL 查询，避免 Entity Framework 的 DBNull 问题
                _logger.LogInformation("Searching for existing user with openId: {openId}", openId);

                var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                // 查询是否存在用户
                using var selectCommand = connection.CreateCommand();
                selectCommand.CommandText = "SELECT ID FROM user WHERE openID = @openId LIMIT 1";
                var selectParam = selectCommand.CreateParameter();
                selectParam.ParameterName = "@openId";
                selectParam.Value = openId;
                selectCommand.Parameters.Add(selectParam);

                var existingUserId = await selectCommand.ExecuteScalarAsync();

                // 3. 如果存在，直接返回 ID
                if (existingUserId != null && existingUserId != DBNull.Value)
                {
                    var userId = Convert.ToInt32(existingUserId);
                    _logger.LogInformation("Found existing user with ID: {userId}", userId);
                    return new UserLoginResponseDto
                    {
                        UserId = userId,
                        IsNewUser = false
                    };
                }

                // 4. 如果不存在，插入新用户
                _logger.LogInformation("Creating new user with openId: {openId}", openId);

                using var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = "INSERT INTO user (openID, name, gender) VALUES (@openId, NULL, NULL); SELECT LAST_INSERT_ID();";
                var insertParam = insertCommand.CreateParameter();
                insertParam.ParameterName = "@openId";
                insertParam.Value = openId;
                insertCommand.Parameters.Add(insertParam);

                var newUserId = await insertCommand.ExecuteScalarAsync();
                var newUserIdInt = Convert.ToInt32(newUserId);

                _logger.LogInformation("Created new user with ID: {userId}", newUserIdInt);

                return new UserLoginResponseDto
                {
                    UserId = newUserIdInt,
                    IsNewUser = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error during user login process. OpenId: {openId}", openId);
                throw new Exception($"数据库操作失败: {ex.Message}");
            }
        }

        public async Task<int> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            try
            {
                var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                var updateParts = new List<string>();
                var command = connection.CreateCommand();

                if (!string.IsNullOrEmpty(userUpdateDto.Name))
                {
                    updateParts.Add("name = @name");
                    var nameParam = command.CreateParameter();
                    nameParam.ParameterName = "@name";
                    nameParam.Value = userUpdateDto.Name;
                    command.Parameters.Add(nameParam);
                }

                if (userUpdateDto.Gender.HasValue)
                {
                    updateParts.Add("gender = @gender");
                    var genderParam = command.CreateParameter();
                    genderParam.ParameterName = "@gender";
                    genderParam.Value = userUpdateDto.Gender.Value;
                    command.Parameters.Add(genderParam);
                }

                if (updateParts.Count == 0)
                {
                    return userUpdateDto.Id; // 没有字段需要更新
                }

                command.CommandText = $"UPDATE user SET {string.Join(", ", updateParts)} WHERE ID = @id";
                var idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = userUpdateDto.Id;
                command.Parameters.Add(idParam);

                var result = await command.ExecuteNonQueryAsync();

                if (result == 0)
                {
                    throw new Exception($"用户不存在，ID: {userUpdateDto.Id}");
                }

                return userUpdateDto.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {userId}", userUpdateDto.Id);
                throw new Exception($"更新用户信息失败: {ex.Message}");
            }
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {
            try
            {
                var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT ID, openID, name, gender FROM user WHERE ID = @userId";
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@userId";
                parameter.Value = userId;
                command.Parameters.Add(parameter);

                using var reader = await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    return null;
                }

                return new UserResponseDto
                {
                    Id = reader.GetInt32("ID"),
                    OpenId = reader.IsDBNull("openID") ? null : reader.GetString("openID"),
                    Name = reader.IsDBNull("name") ? null : reader.GetString("name"),
                    Gender = reader.IsDBNull("gender") ? null : reader.GetInt32("gender")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {userId}", userId);
                throw new Exception($"获取用户信息失败: {ex.Message}");
            }
        }
    }
}