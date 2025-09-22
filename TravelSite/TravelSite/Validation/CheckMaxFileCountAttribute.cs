using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TravelSite.Models.TravelDates;

namespace TravelSite.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class CheckMaxFileCountAttribute : ValidationAttribute, IClientModelValidator
	{
		private readonly int _maxFileCount;
		private readonly string _fileType;
		public CheckMaxFileCountAttribute(int maxFileCount, string fileType)
		{
			ErrorMessage = $"Можно добавить не более {maxFileCount} файлов";
			_maxFileCount = maxFileCount;
			_fileType = fileType;
		}
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			IEnumerable<IFormFile>? files = value as IEnumerable<IFormFile>;
			if (files?.Count() > _maxFileCount)
				return new ValidationResult(ErrorMessage);
			else
				return ValidationResult.Success;
		}
		public void AddValidation(ClientModelValidationContext context)
		{
			var count = _maxFileCount.ToString();
			MergeAttribute(context.Attributes, "data-val", "true");
			MergeAttribute(context.Attributes, "data-val-checkmaxfilecount", ErrorMessage);
			MergeAttribute(context.Attributes, "data-val-checkmaxfilecount-count", count);
			MergeAttribute(context.Attributes, "data-val-checkmaxfilecount-filetype", _fileType);
		}
		private bool MergeAttribute(IDictionary<string, string> attributes,string key,string value)
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
