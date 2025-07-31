using System.ComponentModel.DataAnnotations;
using TravelSite.Models.Roles;

namespace TravelSite.Models.Account
{
    public class RegisterViewModel
	{
		[Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Имя", Prompt = "Введите имя")]
		public string FirstName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Фамилия", Prompt = "Введите фамилию")]
		public string LastName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Поле Email обязательно для заполнения")]
		[EmailAddress]
		[Display(Name = "Email", Prompt = "Введите email")]
		public string EmailReg { get; set; } = string.Empty;

		[Required(ErrorMessage = "Поле дата рождения обязательно для заполнения")]
		[Display(Name = "Дата рождения")]
		public DateTime BirthDate { get; set; }

		[Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль", Prompt = "Введите пароль")]
		[StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 10)]
		public string PasswordReg { get; set; } = string.Empty;

		[Required(ErrorMessage = "Обязательно подтвердите пароль")]
		[Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердить пароль", Prompt = "Подтвердите пароль")]
		public string PasswordConfirm { get; set; } = string.Empty;

		public string Login => EmailReg;
		public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
	}
}
