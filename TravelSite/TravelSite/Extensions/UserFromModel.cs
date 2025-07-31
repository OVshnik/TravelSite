using TravelSite.Data.Models;
using TravelSite.Models.Account;

namespace TravelSite.Extensions
{
	public static class UserFromModel
	{
		public static User Convert(this User user, UserEditViewModel viewModel)
		{
			if (!string.IsNullOrEmpty(viewModel.LastName))
			{
				user.LastName = viewModel.LastName;
			}
			if (!string.IsNullOrEmpty(viewModel.MiddleName))
			{
				user.MiddleName = viewModel.MiddleName;
			}
			if (!string.IsNullOrEmpty(viewModel.FirstName))
			{
				user.FirstName = viewModel.FirstName;
			}
			if (!string.IsNullOrEmpty(viewModel.Email))
			{
				user.Email = viewModel.Email;
			}
			user.BirthDate = viewModel.BirthDate;
			if (!string.IsNullOrEmpty(viewModel.UserName))
			{
				user.UserName = viewModel.UserName;
			}
			return user;
		}
	}
}
