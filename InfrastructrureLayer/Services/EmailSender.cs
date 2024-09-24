using ApplicationLayer.DTOs.Email;
using ApplicationLayer.EmailService;
using ApplicationLayer.Settings;
using System.Net;
using System.Net.Mail;

namespace InfrastructrureLayer.Services {
	public class EmailSender : IEmailSender {
		private readonly EmailSetting _emailSetting;

		public EmailSender(EmailSetting emailSetting) {
			_emailSetting = emailSetting;
		}

		public void SendEmail(Message message) {
			MailMessage mailMessage = new MailMessage() {
				From = new MailAddress(_emailSetting.From),
				Subject = message.Subject,
				Body = message.Content
			};

			mailMessage.To.Add(message.To);

			using var smtpClient = new SmtpClient();
			smtpClient.Host = _emailSetting.Host;
			smtpClient.Port = _emailSetting.Port;
			smtpClient.Credentials = new NetworkCredential(_emailSetting.Username, _emailSetting.Password);
			smtpClient.EnableSsl = true;
			smtpClient.Send(mailMessage);
		}
	}
}
