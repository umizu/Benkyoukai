using Benkyoukai.Services.Contracts.Email;

namespace Benkyoukai.Services.Email.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailRegisterMessageDto message);
}
