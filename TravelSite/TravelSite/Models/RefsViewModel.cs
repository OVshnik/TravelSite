using System.ComponentModel.DataAnnotations;

namespace TravelSite.Models
{
	public class RefsViewModel
	{
		[UrlAttribute]
		[Display(Prompt = "Добавить ссылку на telegram")]
		public string Url1 { get; set; }=String.Empty;
		[UrlAttribute]
		[Display(Prompt = "Добавить ссылку на whatsup")]
		public string Url2 { get; set; } = String.Empty;
	}
}
