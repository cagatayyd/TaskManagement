using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Services.Email
{
	public interface IEmailService
	{
		Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
		Task SendTaskAssignedEmail(string Name, string Surname, string assignedTaskLink, string toEmail);
	}
}
