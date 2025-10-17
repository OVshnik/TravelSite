namespace TravelSite.Services
{
	public interface IEmailService
	{
		public Task SendEmailAsync(string recipientEmail, string senderEmail, string password, string message, string subject);
	}
}
