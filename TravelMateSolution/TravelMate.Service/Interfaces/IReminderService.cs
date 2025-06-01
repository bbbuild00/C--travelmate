using TravelMate.Core.Entities;

namespace TravelMate.Service.Interfaces
{
    // 提醒服务接口
    public interface IReminderService
    {
        Task<int> AddReminderAsync(Reminder reminder);
        Task<int> ModifyReminderAsync(Reminder reminder);
        Task<bool> DeleteReminderAsync(int id);
        Task<List<Reminder>> GetRemindersByUserIdAsync(int userId);
        Task<Reminder?> GetReminderByIdAsync(int id);
    }
}