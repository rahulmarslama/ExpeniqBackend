using Expendiq.Application.Dtos;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;

namespace IdentityServer.IdentityExtensions.Email
{
    public class EmailSender : IEmailSender
    {

        /// <summary>
        /// Email configuration settings. In a production environment, these should be stored securely, 
        /// such as in environment variables or a secure secrets manager, rather than hardcoding them in the code.
        /// </summary>
        private const string FromEmail = "rahullamaaf@gmail.com";
        private const string FromEmailPassword = "zfeg ptim jozf elyl";
        private const string Host = "smtp.gmail.com";
        private const int Port = 587;

        public async Task<DataResult> SendEmailAsync(string toEmail, string subject, string message, string name = null)
        {
            if (toEmail == null)
            {
                return new DataResult
                {
                    Success = false,
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Invalid to email address"
                };
            }

            MimeMessage mimeMessage = new();
            mimeMessage.From.Add(new MailboxAddress("System Admin", FromEmail));
            mimeMessage.To.Add(new MailboxAddress(name ?? "System User", toEmail));
            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart("html") { Text = message };


            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    await client.ConnectAsync(Host, Port);
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Authenticate(new NetworkCredential(FromEmail, FromEmailPassword));
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
                return new DataResult
                {
                    Success = true,
                    Message = "Successfully sent"
                };
            }
            catch (Exception ex)
            {
                return new DataResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
