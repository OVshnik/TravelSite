using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TravelSite.Data.Models;
using TravelSite.Models.TravelPhoto;
using TravelSite.Validation;

namespace TravelSite.Models.Travels
{
	public class CreateTravelViewModel
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Название", Prompt = "Введите название")]
		public string Name { get; set; } = string.Empty;
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Описание", Prompt = "Введите описание")]
		public string Description { get; set; } = string.Empty;
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Категория", Prompt = "Введите название категории")]
		public string Category { get; set; } = string.Empty;
		[NotNull()]
		[Display(Prompt = "Добавьте основное фото")]
		public IFormFile ?Photo { get; set; } 
		[Display(Name = "Видео", Prompt = "Добавьте ссылку на видео")]
		[CheckMaxFileCount(2, "video")]
		public IEnumerable<IFormFile> ?VideoList { get; set; }
		[CheckMaxFileCount(5, "photo")]
		public IEnumerable<IFormFile> ?PhotoGallery { get; set; }
	} 
}
