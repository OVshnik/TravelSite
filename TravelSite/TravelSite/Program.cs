using TravelSite.Data.Models;
using TravelSite.Data;
using TravelSite.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TravelSite.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TravelSite;
using GleamTech.AspNet.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using TravelSite.Notifications;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddGleamTech();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(connection)).AddIdentity<User, Role>(opts =>
			{
				opts.Password.RequiredLength = 5;
				opts.Password.RequireNonAlphanumeric = false;
				opts.Password.RequireLowercase = false;
				opts.Password.RequireUppercase = false;
				opts.Password.RequireDigit = false;
			}).AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

builder.Services.AddTransient<IOrderRepository, OrderRepository>()
	.AddTransient<ITravelRepository, TravelRepository>()
	.AddTransient<IBookingRepository, BookingRepository>()
	.AddScoped<ITravelDatesRepository, TravelDatesRepository>()
	.AddScoped<IPhotoRepository, PhotoRepository>()
	.AddScoped<IVideoRepository, VideoRepository>()
	.AddScoped<INotificationRepository<BookingNotification>, BookingNotificationRepository>();

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ITravelService, TravelService>();
builder.Services.AddTransient<ITravelDatesService, TravelDatesService>();
builder.Services.AddTransient<IBookingService, BookingService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<INotificationService, NotificationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
	options.LoginPath = "/Login";
	options.AccessDeniedPath = "/Login";
});
builder.Services.AddAuthorizationBuilder()
	.AddPolicy("Admin", policy =>
	{
		policy.RequireClaim(ClaimTypes.Role, "Admin", "UberAdmin");
	});
builder.Services.AddRazorPages()
	.AddViewOptions(options =>
	{
		options.HtmlHelperOptions.ClientValidationEnabled = true;
	});

builder.Services.AddSignalR();
builder.Services.AddTransient<NotificationHub>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.MapHub<NotificationHub>("/Notification");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseGleamTech();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
