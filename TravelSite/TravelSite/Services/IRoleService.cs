using TravelSite.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TravelSite.Models.Roles;

namespace TravelSite.Services;

public interface IRoleService
{
	public Task<IdentityResult> CreateRoleAsync(CreateRoleViewModel model);
	public Task<IdentityResult> UpdateRoleAsync(EditRoleViewModel model);
	public Task<RoleViewModel> GetRoleAsync(string id);
	public Task<List<RoleViewModel>> GetAllRolesAsync();
	public Task DeleteRoleAsync(string id, ClaimsPrincipal claims);
	public Task<EditRoleViewModel> EditRoleAsync(string id);
	public Task<Role> CheckNameAsync(string name);
	public Task CreateBaseRoles();
}
