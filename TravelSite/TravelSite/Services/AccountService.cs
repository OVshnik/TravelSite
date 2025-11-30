using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TravelSite.Data.Models;
using TravelSite.Extensions;
using TravelSite.Models;
using TravelSite.Models.Account;
using TravelSite.Models.Roles;

namespace TravelSite.Services
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<Role> _roleManager;
		private readonly IMapper _mapper;
		private readonly IFileService _fileService;
		public AccountService(UserManager<User> userManager, 
			SignInManager<User> signInManager, 
			RoleManager<Role> roleManager, 
			IMapper mapper,
			IFileService fileService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_mapper = mapper;
			_fileService = fileService;
		}
	
		public async Task<IdentityResult> DeleteUserAsync(string id)
		{
			var user= await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				var result = await _userManager.DeleteAsync(user);
				return result;
			}
			throw new Exception($"Пользователя с id={id} не удалось получить из БД");
		}

		public async Task<UserEditViewModel> EditUser(ClaimsPrincipal claims)
		{
			var user=await _userManager.GetUserAsync(claims);
			var roles=_roleManager.Roles.ToList();

			var model=_mapper.Map<UserEditViewModel>(user);

			foreach(var role in roles)
			{
				model.Roles.Add(_mapper.Map<RoleViewModel>(role));
			}
			return model;
		}

		public async Task<UserEditViewModel> EditUser(string id)
		{
			var user=await _userManager.FindByIdAsync(id);

			if(user!=null)
			{
				var userRoles=await _userManager.GetRolesAsync(user);

				var roles=_roleManager.Roles.ToList();

				var model = _mapper.Map<UserEditViewModel>(user);

				foreach (var role in roles)
				{
						var rm = _mapper.Map<RoleViewModel>(role);
						if (userRoles.Contains(role.Name))
						{
							rm.IsChecked = true;
							model.Roles.Add(rm);
						}
						else
						{
							model.Roles.Add(rm);
						}
				}
				return model;
			}
			throw new Exception($"Пользователя с id={id} не удалось получить из БД");
		}

		public async Task<List<UserViewModel>> GetAllUsersAsync()
		{
			var users= await _userManager.Users.AsQueryable().Include(x => x.Bookings).ToListAsync();
			var listUsers= new List<UserViewModel>();
			if (users != null)
			{
				foreach (var user in users)
				{
					var u = _mapper.Map<UserViewModel>(user);
					listUsers.Add(u);
				}
				
			}
			return listUsers;
		}

		public async Task<UserViewModel> GetCurrentUserAsync(ClaimsPrincipal claims)
		{
			var user = await _userManager.GetUserAsync(claims);
			if(user!=null)
			{
				var model = _mapper.Map<UserViewModel>(user);

				return model;
			}
			throw new Exception($"Пользователя с именем {claims.Identity?.Name} не удалось получить из БД");
		}

		public async Task<UserViewModel> GetUserAsync(string id)
		{
			var user=await _userManager.FindByIdAsync(id);
			if(user!=null)
			{
				var model=_mapper.Map<UserViewModel>(user);

				var roles = await _userManager.GetRolesAsync(user);

				return model;
			}
			throw new Exception($"Пользователя с id={id} не удалось получить из БД");
		}

		public async Task<SignInResult> Login(LoginViewModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if(user!=null)
			{
				var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,false);
				if(result.Succeeded)
				{
					if(user.Email!=null)
						await _userManager.AddClaimAsync(user,new Claim("Email",user.Email));	
					return result;
				}
			}
			return SignInResult.Failed;
		}

		public RegisterViewModel Register()
		{
			var model=new RegisterViewModel();
			var roles = _roleManager.Roles.ToList();
			foreach (var role in roles)
			{
				model.Roles.Add(_mapper.Map<RoleViewModel>(role));
			}
			return model;
		}

		public async Task<IdentityResult> Register(RegisterViewModel model)
		{
			if (!model.EmailKey.IsNullOrEmpty())
			{
				model.EmailKey = HT.Core.CryptExtensions.Encrypt(model.EmailKey);
			}
			var user = _mapper.Map<User>(model);
			var result = await _userManager.CreateAsync(user, model.PasswordReg);
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user,false);
				await _userManager.AddToRoleAsync(user, "User");
				return result;
			}
			return result;
		}

		public async Task<IdentityResult> UpdateUserAsync(UserEditViewModel model)
		{
			var user=await _userManager.FindByIdAsync(model.UserId);
			if(user!=null)
			{
				user.Convert(model);
				if(model.Roles.Count>0)
				{
					foreach (var role in model.Roles)
					{
						if(role.IsChecked)
							await _userManager.AddToRoleAsync(user,role.Name);
						else
						{
							await _userManager.RemoveFromRoleAsync(user, role.Name);
						}
					}
				}
				var result = await _userManager.UpdateAsync(user);
				return result;
			}
			throw new Exception($"Пользователя с id={model.UserId} не удалось получить из БД");
		}
		public async Task CreateAdmin()
		{
			var newUser = new User
			{
				FirstName = "Admin",
				LastName = "Adminov",
				Email = "admin@gmail.com",
				BirthDate = new DateTime(1995, 01, 11),
				UserName = "admin@gmail.com"
			};

			if (await _userManager.FindByIdAsync(newUser.Id) == null)
			{
				var result = await _userManager.CreateAsync(newUser, "admin1995");
				if (result.Succeeded)
					await _userManager.AddToRoleAsync(newUser, "Admin");
			}
		}
		public async Task AddRefs(RefsViewModel model)
		{
			await _fileService.SaveFileInFolder("wwwroot/etc", model.Url1, ".txt", "telegramRef");
			await _fileService.SaveFileInFolder("wwwroot/etc", model.Url2, ".txt", "whatsupRef");
		}
		public async Task<RefsViewModel> AddRefs()
		{
			var model=new RefsViewModel();
			var url1=await _fileService.ReadFileInFolder("wwwroot/etc", ".txt", "telegramRef");
			var url2=await _fileService.ReadFileInFolder("wwwroot/etc", ".txt", "whatsupRef");
			if(url1 != null && url2 != null)
			{
				model.Url1 = url1;
				model.Url2 = url2;
			}
			return model;
		}
	}
}
