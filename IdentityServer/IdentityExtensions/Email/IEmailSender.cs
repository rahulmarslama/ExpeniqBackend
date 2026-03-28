using Expendiq.Application.Dtos;

namespace IdentityServer.IdentityExtensions.Email
{
    public interface IEmailSender
    {
        Task<DataResult> SendEmailAsync(string toEmail, string subject, string message, string name = null);
    }
}
