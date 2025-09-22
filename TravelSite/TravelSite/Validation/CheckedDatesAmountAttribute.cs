using System.ComponentModel.DataAnnotations;
using TravelSite.Data.Models;
using TravelSite.Models.TravelDates;

namespace TravelSite.Validation
{
	public class CheckedDatesAmountAttribute:ValidationAttribute
	{
		public CheckedDatesAmountAttribute()
		{
			ErrorMessage = "Можно выбрать только один вариант дат в рамках одного бронирования";
		}
		public override bool IsValid(object? value)
		{
			List<TravelDatesViewModel>? dates = value as List<TravelDatesViewModel>;

			if (dates?.Where(c => c.isChecked == true).ToList().Count > 1)
			{
				return false;
			}
			return true;
		}
	}
}
