using TravelMate.Core.Entities;
using TravelMate.Data;
using TravelMate.Service.Interfaces;

namespace TravelMate.Service.Implementations
{
	public class UserService : IUserService
	{
		private readonly TravelMateDbContext _dbContext;

		public UserService(TravelMateDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public User GetUserById(int userId)
		{
			return _dbContext.Users.FirstOrDefault(u => u.Id == userId);
		}
	}
}
