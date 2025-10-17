using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelSite.Data.Models;
using TravelSite.Models;
using TravelSite.Models.Travels;
using TravelSite.Services;

namespace TravelSite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IRoleService _roleService;
		private readonly IAccountService _accountService;
		private readonly ITravelService _travelService;
		private readonly IMapper _mapper;
		private readonly UserManager<User> _userManager;

		public HomeController(ILogger<HomeController> logger,
			IRoleService roleService,
			IAccountService accountService,
			ITravelService travelService,
			IMapper mapper,
			UserManager<User> userManager)
		{
			_logger = logger;
			_roleService = roleService;
			_accountService = accountService;
			_travelService = travelService;
			_mapper = mapper;
			_userManager = userManager;
		}
		public async Task<IActionResult> Index()
		{
			await _roleService.CreateBaseRoles();
			await _accountService.CreateAdmin();
			var travels = await _travelService.GetAllTravelAsync();
			var trList = new List<TravelViewModel>();
			foreach (var travel in travels)
			{
				trList.Add(_mapper.Map<TravelViewModel>(travel));
			}
			var model = new HomeViewModel(trList);
			return View(model);
		}
		public IActionResult Help()
		{
			return View();
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
