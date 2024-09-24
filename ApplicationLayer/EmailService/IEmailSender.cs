using ApplicationLayer.DTOs.Email;

namespace ApplicationLayer.EmailService {
	public interface IEmailSender {
		void SendEmail(Message message);
	}
}
