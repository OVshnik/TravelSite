using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelSite.Data.Models;

namespace TravelSite.Data
{
	public class ApplicationDbContext:IdentityDbContext<User,Role,string>
	{
		public DbSet<Order> Orders { get; set; }
		public DbSet<Travel> Travels { get; set; }
		public DbSet<Booking> Bookings { get; set; }
		public DbSet<TravelDates> TravelDates { get; set; }
		public DbSet<TravelPhoto> TravelPhoto { get; set; }
		public DbSet<TravelVideo> TravelVideo { get; set; }
		public DbSet<BookingNotification> Notifications { get; set; }
		public ApplicationDbContext()
		{
		}
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
		{
			if (Database.GetPendingMigrations().Any())
			{
				Database.Migrate();
			}
			else
			{
				Database.EnsureCreated();
			}
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<TravelDates>()
			.ToTable(t => t.HasCheckConstraint("AvailablePlaces", "AvailablePlaces <= MaxPlaces"));
			builder.Entity<BookingNotification>().HasOne(t => t.Sender).WithMany(x => x.SendNotifications).HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.ClientSetNull);
			builder.Entity<BookingNotification>().HasOne(t => t.Recipient).WithMany(x => x.ReceivedNotifications).HasForeignKey(x => x.RecipientId).OnDelete(DeleteBehavior.ClientSetNull);
			builder.Entity<BookingNotification>().HasOne(x => x.Booking).WithMany(x => x.BookingNotifications).HasForeignKey(x => x.BookingId);
			builder.Entity<TravelDates>().HasMany(x => x.Bookings).WithOne(x => x.TravelDates).HasForeignKey(x => x.TravelDatesId);
		}
	}
}
