using System.Net.Mail;
using System.Net;

namespace MyNoteSampleApp.Core
{
    public class MailHelper
    {
        public void SendMail(string subject, string body, params string[] tos)
        {
            SmtpClient client = new SmtpClient("smtp.mail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("admin@mail.com", "123456")
            };

            MailMessage mailMessage = new MailMessage()
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };

            mailMessage.From = new MailAddress("admin@mail.com");

            foreach (string emailAddress in tos)
            {
                mailMessage.To.Add(emailAddress);
            }

            client.Send(mailMessage);
        }
    }
}
