using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TravelSite.Data.Models;
using TravelSite.Models.TravelPhoto;
using TravelSite.Models.TravelVideo;
using TravelSite.Validation;

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
		public string Photo { get; set; }= string.Empty;
		[Display(Prompt = "Добавьте основное фото")]
		public IFormFile? EditPhoto { get; set; }
		public List<PhotoViewModel> PhotoGallery { get; set; } = new List<PhotoViewModel>();
		public List<VideoViewModel> VideoList { get; set; } = new List<VideoViewModel>();

		[CheckMaxFileCount(5,"photo")]
		public IEnumerable<IFormFile> ?PhotoFiles { get; set; }
		[CheckMaxFileCount(2, "video")]
		public IEnumerable<IFormFile> ?VideoFiles { get; set; }
	}
}
