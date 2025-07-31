using System.ComponentModel.DataAnnotations;

namespace TravelSite.Models.Account
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[EmailAddress]
		[Display(Name = "Email", Prompt = "Введите email")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль", Prompt = "Введите пароль")]
		public string Password { get; set; } = string.Empty;

		[Display(Name = "Запомнить?")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; } = "";
	}
}
