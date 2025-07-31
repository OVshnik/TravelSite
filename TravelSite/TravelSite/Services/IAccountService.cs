using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TravelSite.Models.Account;

namespace TravelSite.Services
{
	public interface IAccountService
	{
		public Task<IdentityResult> UpdateUserAsync(UserEditViewModel model);
		public Task<UserViewModel> GetUserAsync(string id);
		public Task<List<UserViewModel>> GetAllUsersAsync();
		public Task<IdentityResult> DeleteUserAsync(string id);
		public RegisterViewModel Register();
		public Task<IdentityResult> Register(RegisterViewModel model);
		public Task<SignInResult> Login(LoginViewModel model);
		public Task<UserEditViewModel> EditUser(ClaimsPrincipal claims);
		public Task<UserEditViewModel> EditUser(string id);
		public Task<UserViewModel> GetCurrentUserAsync(ClaimsPrincipal claims);
		public Task CreateAdmin();
	}
}
