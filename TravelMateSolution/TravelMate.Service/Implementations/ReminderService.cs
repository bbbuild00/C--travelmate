using Microsoft.EntityFrameworkCore;
using TravelMate.Core.Entities;
using TravelMate.Data;
using TravelMate.Service.Interfaces;
using TravelMate.Interop;
using Microsoft.Extensions.Logging;

namespace TravelMate.Service.Implementations
{
    // 提醒服务实现
    public class ReminderService : IReminderService
    {
        private readonly TravelMateDbContext _context;
        private readonly ILogger<ReminderService> _logger;

        public ReminderService(TravelMateDbContext context, ILogger<ReminderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> AddReminderAsync(Reminder reminder)
        {
            try
            {
                // 使用 C++ Interop 验证提醒数据
                var managedReminder = new ManagedReminderData
                {
                    Id = reminder.Id,
                    Time = reminder.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    Location = reminder.Location,
                    UserId = reminder.UserId,
                    Date = reminder.Date.ToString("yyyy-MM-dd")
                };

                if (!WeatherInterop.ValidateReminderData(managedReminder))
                {
                    throw new ArgumentException("Invalid reminder data");
                }

                // 计算哈希值用于日志
                var hash = WeatherInterop.CalculateReminderHash(managedReminder);
                _logger.LogInformation($"Adding reminder with hash: {hash}");

                // 保存到数据库
                _context.Reminders.Add(reminder);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully added reminder with ID: {reminder.Id}");
                return reminder.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reminder");
                throw new Exception($"Error adding reminder: {ex.Message}", ex);
            }
        }

        public async Task<int> ModifyReminderAsync(Reminder reminder)
        {
            try
            {
                // 查找现有记录
                var existingReminder = await _context.Reminders.FindAsync(reminder.Id);
                if (existingReminder == null)
                {
                    throw new ArgumentException($"Reminder with ID {reminder.Id} not found");
                }

                // 更新字段 - 只更新非默认值的字段
                if (reminder.Time != default(DateTime))
                    existingReminder.Time = reminder.Time;

                if (!string.IsNullOrEmpty(reminder.Location))
                    existingReminder.Location = reminder.Location;

                if (reminder.UserId > 0)
                    existingReminder.UserId = reminder.UserId;

                if (reminder.Date != default(DateTime))
                    existingReminder.Date = reminder.Date;

                // 用完整的现有数据进行 C++ 验证
                var managedReminder = new ManagedReminderData
                {
                    Id = existingReminder.Id,
                    Time = existingReminder.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                    Location = existingReminder.Location,
                    UserId = existingReminder.UserId,
                    Date = existingReminder.Date.ToString("yyyy-MM-dd")
                };

                if (!WeatherInterop.ValidateReminderData(managedReminder))
                {
                    _logger.LogWarning($"C++ validation failed for reminder ID: {reminder.Id}");
                    // 不抛出异常，只记录警告，继续执行数据库更新
                }

                // 计算哈希值用于日志
                var hash = WeatherInterop.CalculateReminderHash(managedReminder);
                _logger.LogInformation($"Modifying reminder with hash: {hash}");

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully modified reminder with ID: {reminder.Id}");
                return reminder.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error modifying reminder with ID: {reminder.Id}");
                throw new Exception($"Error modifying reminder: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteReminderAsync(int id)
        {
            try
            {
                var reminder = await _context.Reminders.FindAsync(id);
                if (reminder == null)
                {
                    _logger.LogWarning($"Reminder with ID {id} not found for deletion");
                    return false;
                }

                _context.Reminders.Remove(reminder);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully deleted reminder with ID: {id}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting reminder with ID: {id}");
                throw new Exception($"Error deleting reminder: {ex.Message}", ex);
            }
        }

        public async Task<List<Reminder>> GetRemindersByUserIdAsync(int userId)
        {
            try
            {
                var reminders = await _context.Reminders
                    .Where(r => r.UserId == userId)
                    .OrderBy(r => r.Date)
                    .ThenBy(r => r.Time)
                    .ToListAsync();

                _logger.LogInformation($"Retrieved {reminders.Count} reminders for user {userId}");
                return reminders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reminders for user {userId}");
                throw new Exception($"Error getting reminders: {ex.Message}", ex);
            }
        }

        public async Task<Reminder?> GetReminderByIdAsync(int id)
        {
            try
            {
                var reminder = await _context.Reminders.FindAsync(id);
                if (reminder != null)
                {
                    _logger.LogInformation($"Found reminder with ID: {id}");
                }
                else
                {
                    _logger.LogWarning($"Reminder with ID {id} not found");
                }
                return reminder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting reminder with ID: {id}");
                throw new Exception($"Error getting reminder: {ex.Message}", ex);
            }
        }
    }
}