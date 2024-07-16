using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Services.Email
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;

		public EmailService(IOptions<EmailSettings> options)
		{
			_emailSettings = options.Value;
		}

		public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail)
		{
			var smtpClient = new SmtpClient();

			smtpClient.Host = _emailSettings.Host;
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Port = 587;
			smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
			smtpClient.EnableSsl = true;

			var mailMessage = new MailMessage();

			mailMessage.From = new MailAddress(_emailSettings.Email);
			mailMessage.To.Add(toEmail);

			mailMessage.Subject = "Localhost | Şifre sıfırlama linki";
			mailMessage.Body = @$"
                               <h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h4>
                               <a href='{resetPasswordEmailLink}'>Şifre yenileme linki</a>";

			mailMessage.IsBodyHtml = true;
			await smtpClient.SendMailAsync(mailMessage);

		}
		public async Task SendTaskAssignedEmail(string name, string surname, string assignedTaskLink, string toEmail)
		{
			var smtpClient = new SmtpClient();

			smtpClient.Host = _emailSettings.Host;
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Port = 587;
			smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
			smtpClient.EnableSsl = true;

			var mailMessage = new MailMessage();

			mailMessage.From = new MailAddress(_emailSettings.Email);
			mailMessage.To.Add(toEmail);

			mailMessage.Subject = "Task Management | Görev Bildirimi";
			mailMessage.Body = @$"
                               <h2>Task Management</h2>
                                <hr>
                                <h4> Sayın '{name + " " + surname}' , sizin adınıza bir görev atanmıştır sitemize giderek kontrol edebilirsiniz. </h4>
                                <a href='{assignedTaskLink}'>Görev görüntüleme linki</a>"
								;

			mailMessage.IsBodyHtml = true;
			await smtpClient.SendMailAsync(mailMessage);

		}
	}

}
