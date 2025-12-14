using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TravelSite.Data.Models;
using TravelSite.Models;
using TravelSite.Models.Account;
using TravelSite.Services;

namespace TravelSite.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly IAccountService _accountService;
		private readonly ILogger<AccountController> _logger;
		public AccountController(SignInManager<User> signInManager, IAccountService accountService, ILogger<AccountController> logger)
		{
			_signInManager = signInManager;
			_accountService = accountService;
			_logger = logger;
		}
		/// <summary>
		/// [Get] Метод, логирования пользователя
		/// </summary>
		[Route("Login")]
		[HttpGet]
		public IActionResult Login()
		{
			return View("Login");
		}
		/// <summary>
		/// [Post] Метод, выход пользователя
		/// </summary>
		[Route("Logout")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
		/// <summary>
		/// [Get] Метод, логирования пользователя
		/// </summary>
		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}
		/// <summary>
		/// [Post] Метод, логирования пользователя
		/// </summary>
		[Route("Login")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _accountService.Login(model);

				if (result.Succeeded)
				{
					_logger.LogInformation($"Пользователь с логином {model.Email} вошел в систему", model.Email);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Неправильный логин и (или) пароль");
				}
			}
			return RedirectToAction("Login",new {});
		}
		/// <summary>
		/// [Get] Метод, регистрации пользователя
		/// </summary>
		[Route("Register")]
		[HttpGet]
		public IActionResult Register()
		{
			var model = _accountService.Register();
			return View("RegisterPage", model);
		}
		/// <summary>
		/// [Post] Метод, регистрации пользователя
		/// </summary>
		[Route("Register")]
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _accountService.Register(model);
				_logger.LogInformation($"Пользователь с логином {model.EmailReg} зарегистрирован", model.EmailReg);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				foreach (var item in ModelState)
				{
					foreach (var error in item.Value.Errors)
					{
						Console.WriteLine(error.ErrorMessage);
					}

				}
				return View("RegisterPage");
			}
		}
		/// <summary>
		/// [Get] Метод, для входа на страницу пользователя
		/// </summary>
		[Authorize]
		[Route("MyPage")]
		[HttpGet]
		public async Task<IActionResult> MyPage()
		{
			var model = await _accountService.GetCurrentUserAsync(User);

			return View("UserPage", model);
		}
	    /// <summary>
		/// [Get] Метод, для входа на страницу пользователя по его id
		/// </summary>
		[Authorize]
		[Route("UserPage")]
		[HttpGet]
		public async Task<IActionResult> GetUser(string id)
		{
			var model = await _accountService.GetUserAsync(id);

			return View("UserPage", model);
		}
		/// <summary>
		/// [Get] Метод, для перехода на страницу с отображением всех пользователей
		/// </summary>
		[Authorize]
		[Route("UserList")]
		[HttpGet]
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _accountService.GetAllUsersAsync();
			return View("UserList", users);
		}
		/// <summary>
		/// [Get] Метод, для редактирования страницы пользователя
		/// </summary>
		[Route("EditUser")]
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> EditUser()
		{
			var user = User;

			var model = await _accountService.EditUser(user);

			return View("EditUser", model);
		}
		/// <summary>
		/// [Post] Метод, для редактирования страницы пользователя с правами администратора
		/// </summary>
		[Route("AdminEditUser")]
		[Authorize("Admin")]
		[HttpPost]
		public async Task<IActionResult> AdminEditUser(string id)
		{
			var model = await _accountService.EditUser(id);

			return View("EditUser", model);
		}
		/// <summary>
		/// [Post] Метод, для обновления данных пользователя
		/// </summary>
		[Route("UpdateUser")]
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> UpdateUser(UserEditViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _accountService.UpdateUserAsync(model);
				if (result.Succeeded)
				{
					_logger.LogDebug($"У пользователя с логином {model.Email} изменены данные", model.Email);
					return RedirectToAction("GetAllUsers");
				}
				else
					return RedirectToAction("EditUser");
			}
			else
			{
				ModelState.AddModelError("", "Некорректные значения");
				string errorMessages = "";
				foreach (var item in ModelState)
				{
					if (item.Value.ValidationState == ModelValidationState.Invalid)
					{
						errorMessages = $"{errorMessages}\nОшибки для свойства {item.Key}:\n";
						foreach (var error in item.Value.Errors)
						{
							errorMessages = $"{errorMessages}{error.ErrorMessage}\n";
						}
					}
				}
				return RedirectToAction("EditUser");
			}
		}
		/// <summary>
		/// [Post] Метод, для удаления пользователя
		/// </summary>
		[Authorize("Admin")]
		[Route("DeleteUser")]
		[HttpPost]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _accountService.GetUserAsync(id);
			await _accountService.DeleteUserAsync(id);

			_logger.LogDebug($"Пользователь {user.User.Email} удален", user.User.Email);
			return RedirectToAction("GetAllUsers");
		}
		/// <summary>
		/// [Get] Метод, для входа на страницу с настройками для администратора
		/// </summary>
		[Authorize("Admin")]
		[Route("AdminSettings")]
		[HttpGet]
		public async Task<IActionResult> AdminSettings()
		{
			return View(await _accountService.AddRefs());
		}
		/// <summary>
		/// [Post] Метод, для добавления ссылок на мессенджеры
		/// </summary>
		[Authorize("Admin")]
		[Route("AddReferences")]
		[HttpPost]
		public IActionResult AddRefs(RefsViewModel model)
		{
			_accountService.AddRefs(model);
			return RedirectToAction("AdminSettings");
		}
		/// <summary>
		/// [Get] Метод, для получения ссылок на мессенджеры
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetUrls()
		{
			var model = await _accountService.AddRefs();
			return Json(model);
		}
	}
}