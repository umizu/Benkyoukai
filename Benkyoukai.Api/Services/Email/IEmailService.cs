using Benkyoukai.Services.Contracts.Email;

namespace Benkyoukai.Api.Services.Email;

public interface IEmailService
{
    void SendRegistrationEmail(EmailRegisterMessageDto message);
}
