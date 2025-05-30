using Microsoft.EntityFrameworkCore;
using TravelMate.Core.Entities;

namespace TravelMate.Data
{
	public class TravelMateDbContext : DbContext
	{
		// ✅ 仅保留真正需要数据库表映射的实体
		public DbSet<User> Users { get; set; }
		public DbSet<Itinerary> Itineraries { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<Budget> Budgets { get; set; }
		public DbSet<Expense> Expenses { get; set; }

		public TravelMateDbContext(DbContextOptions<TravelMateDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// ✅ 明确设置数据库对应表名（可选）
			modelBuilder.Entity<Itinerary>().ToTable("itinerary");
			modelBuilder.Entity<Event>().ToTable("event");
			modelBuilder.Entity<Budget>().ToTable("budget");
			modelBuilder.Entity<Expense>().ToTable("expense");

		}
	}
}
