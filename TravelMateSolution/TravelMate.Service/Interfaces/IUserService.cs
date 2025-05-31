using TravelMate.Core.DTOs;

namespace TravelMate.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserLoginResponseDto> LoginAsync(string code);
        Task<int> UpdateUserAsync(UserUpdateDto userUpdateDto);
        Task<UserResponseDto> GetUserByIdAsync(int userId);
    }
}