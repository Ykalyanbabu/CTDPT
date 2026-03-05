using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TGCTDPT.Services
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string to, string subject, string body, bool isHtml = false);
    }

    public class EmailService : IEmailService
    {
        public async Task<bool> SendAsync1(string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(to);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = isHtml;

                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log error
                return false;
            }
        }
        public async Task<bool> SendAsync(string to, string subject, string body, bool isHtml = false)
        {
            System.Net.ServicePointManager.SecurityProtocol =System.Net.SecurityProtocolType.Tls12;
            try
            {
                string FromMail = ConfigurationManager.AppSettings["FromMail"].ToString();
                string Password = ConfigurationManager.AppSettings["FromMailPwd"].ToString();
                string Smptpserver = ConfigurationManager.AppSettings["Smptpserver"].ToString();
                var message = new MailMessage();
                message.From = new MailAddress(FromMail);
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                var smtp = new SmtpClient(Smptpserver, 587);
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(FromMail, Password);

                await smtp.SendMailAsync(message);

                return true;
            }
            catch (Exception ex)
            {
                var error = ex.Message; 
                return false;
            }
        }

    }
}
