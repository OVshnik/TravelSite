using MimeKit;
using MailKit.Net.Smtp;
using System.Text.RegularExpressions;

namespace TravelSite.Services
{
	public class EmailService : IEmailService
	{
		/// <summary>
		/// Метод для отправки уведомления по email
		/// </summary>
		public async Task SendEmailAsync(string recipientEmail, string senderEmail, string password,string message, string subject)
		{
			using var emailMessage = new MimeMessage();

			emailMessage.From.Add(new MailboxAddress("Admin", senderEmail));
			emailMessage.To.Add(new MailboxAddress("", recipientEmail));
			emailMessage.Subject = subject;
			emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = message,
			};
			var userName = Regex.Split(senderEmail, "@")[0];
			using (var client = new SmtpClient())
			{
				await client.ConnectAsync("smtp.gmail.com",465,true);
				await client.AuthenticateAsync(userName, password);
				await client.SendAsync(emailMessage);
				await client.DisconnectAsync(true);
			}

		}
	}
}
