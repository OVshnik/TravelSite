using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using TravelSite.Data.Models;
using TravelSite.Models.TravelDates;

namespace TravelSite.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class CheckDatesAmountAttribute:ValidationAttribute, IClientModelValidator
	{
		private readonly int _maxDateCount;
		public CheckDatesAmountAttribute(int maxDateCount)
		{
			_maxDateCount = maxDateCount;
			ErrorMessage = "Можно выбрать только один вариант дат в рамках одного бронирования";
		}
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			List<TravelDatesViewModel>? dates = value as List<TravelDatesViewModel>;

			if (dates?.Where(c => c.isChecked == true).ToList().Count > 1)
			{
				return new ValidationResult(ErrorMessage);
			}
			return ValidationResult.Success;
		}
		public void AddValidation(ClientModelValidationContext context)
		{
			var count=_maxDateCount.ToString();
			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, "data-val-checkdatesamount", ErrorMessage);
			MergeAttribute(context.Attributes, "data-val-checkdatesamount-count", count);
		}
		private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
		{
			if (attributes.ContainsKey(key))
			{
				return false;
			}
			attributes.Add(key, value);
			return true;
		}
	}
}
