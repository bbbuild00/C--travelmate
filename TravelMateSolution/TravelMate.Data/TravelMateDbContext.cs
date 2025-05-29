using Microsoft.EntityFrameworkCore;
using TravelMate.Core.Entities;

namespace TravelMate.Data
{
	public class TravelMateDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		public TravelMateDbContext(DbContextOptions<TravelMateDbContext> options)
			: base(options)
		{
		}
	}
}
