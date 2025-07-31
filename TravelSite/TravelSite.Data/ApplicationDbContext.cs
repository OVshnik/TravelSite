using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using TravelSite.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace TravelSite.Data
{
	public class ApplicationDbContext:IdentityDbContext<User,Role,string>
	{
		public DbSet<Order> Orders { get; set; }
		public DbSet<Travel> Travels { get; set; }
		public DbSet<Booking> Bookings { get; set; }
		public DbSet<TravelDates> TravelDates { get; set; }
		public DbSet<TravelPhoto> TravelPhoto { get; set; }
		public ApplicationDbContext()
		{
		}
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
		{
			//if (Database.GetPendingMigrations().Any())
			//{
				//Database.Migrate();
			//}
			//else
			//{
			//	Database.EnsureCreated();
			//}
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<TravelDates>()
			.ToTable(t => t.HasCheckConstraint("AvailablePlaces", "AvailablePlaces <= MaxPlaces"));
		}

	}
}
