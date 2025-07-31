using System.ComponentModel.DataAnnotations;
using TravelSite.Data.Models;
using TravelSite.Models.TravelPhoto;

namespace TravelSite.Models.Travels
{
	public class EditTravelViewModel
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
		[DataType(DataType.Text)]
		[Display(Name = "Фото", Prompt = "Добавьте ссылку на фото")]
		public string Photo { get; set; } = string.Empty;
		[DataType(DataType.Text)]
		[Display(Name = "Видео", Prompt = "Добавьте ссылку на видео")]
		public string Video { get; set; } = string.Empty;
		public List<PhotoViewModel> PhotoGallery = new List<PhotoViewModel>();
	}
}
