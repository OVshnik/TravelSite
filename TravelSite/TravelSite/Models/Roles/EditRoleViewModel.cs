using System.ComponentModel.DataAnnotations;

namespace TravelSite.Models.Roles;

    public class EditRoleViewModel
    {
        public string Id { get; set; } = string.Empty;
	    [Required(ErrorMessage = "Поле обязательно для заполнения")]
	    [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Поменять название")]
        public string Name { get; set; } = string.Empty;
	    [Required(ErrorMessage = "Поле обязательно для заполнения")]
	    [DataType(DataType.Text)]
      	[Display(Name = "Описание", Prompt = "Поменять описание")]
	    public string Description { get; set; } = string.Empty;
    }

