using TravelMate.Core.Entities;

namespace TravelMate.Service.Interfaces
{
	public interface IUserService
	{
		User GetUserById(int userId);
	}
}
