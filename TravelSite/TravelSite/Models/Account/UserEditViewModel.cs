using System.ComponentModel.DataAnnotations;
using TravelSite.Models.Roles;

namespace TravelSite.Models.Account
{
	public class UserEditViewModel
	{
		public string UserId { get; set; } = string.Empty;

		[DataType(DataType.Text)]
		[Display(Name = "Имя", Prompt = "Введите имя")]
		public string FirstName { get; set; } = string.Empty;

		[DataType(DataType.Text)]
		[Display(Name = "Фамилия", Prompt = "Введите фамилию")]
		public string LastName { get; set; } = string.Empty;

		[EmailAddress]
		[Display(Name = "Email", Prompt = "example.com")]
		public string Email { get; set; } = string.Empty;

		[DataType(DataType.Date)]
		[Display(Name = "Дата рождения")]
		public DateTime BirthDate { get; set; }

		public string UserName => Email;

		[DataType(DataType.Text)]
		[Display(Name = "Отчество", Prompt = "Введите отчество")]
		public string MiddleName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль", Prompt = "12345")]
		[StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 10)]
		public string Password { get; set; } = string.Empty;

		public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
	}
}
