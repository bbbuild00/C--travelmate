using TravelMate.Core.Entities;

namespace TravelMate.Service.Interfaces
{
    // 天气服务接口
    public interface IWeatherService
    {
        Task<Weather> GetWeatherAsync(string location, DateTime date);
    }
}
