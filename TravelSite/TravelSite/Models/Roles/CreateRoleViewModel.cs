using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TravelSite.Models.Roles;

    public class CreateRoleViewModel
    {
        [Remote(action: "CheckRoleName", controller: "Role", ErrorMessage = "роль с таким именем уже существует в базе")]
        [DataType(DataType.Text)]
	[Required(ErrorMessage = "Поле обязательно для заполнения")]
	[Display(Name = "Роль", Prompt = "Укажите роль")]
        public string Name { get; set; } = string.Empty;
	[DataType(DataType.Text)]
	[Display(Name = "Роль", Prompt = "Добавьте описание роли")]
	[Required(ErrorMessage = "Поле обязательно для заполнения")]
	public string Description { get; set; } = string.Empty;
    }
