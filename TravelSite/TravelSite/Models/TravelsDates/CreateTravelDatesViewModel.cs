using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using TravelSite.Models.Travels;

namespace TravelSite.Models.TravelDates
{
	public class CreateTravelDatesViewModel
	{
		public Guid Id { get; set; }
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[DataType(DataType.Date)]
		[Display(Name = "Дата начала", Prompt = "Укажите дату")]
		public DateOnly From { get; set; }
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[DataType(DataType.Date)]
		[Display(Name = "Дата окончания", Prompt = "Укажите дату")]
		public DateOnly To { get; set; }
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[Range(1, 200, ErrorMessage = "Недопустимое количество")]
		[Display(Name = "Максимальное количество мест", Prompt = "Укажите количество мест")]
		public int MaxPlaces { get; set; }
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[Range(1, 200, ErrorMessage = "Недопустимое количество")]
		[Display(Name = "Количество свободных мест", Prompt = "Укажите количество мест")]
		public int AvailablePlaces { get; set; }
		[Required(ErrorMessage = "Поле обязательно для заполнения")]
		[Range(1, 100000000000, ErrorMessage = "Цена не может быть меньше нуля")]
		[Display(Name = "Стоимость", Prompt = "Укажите цену")]
		public int Price { get; set; }
		public int DaysCount { get; set; }
		public TravelViewModel Travel { get; set; }=new TravelViewModel();
	}
}
