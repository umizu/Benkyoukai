using Benkyoukai.Api.Dtos.Emails;
namespace Benkyoukai.Api.Services.Email;

public interface IEmailService
{
    void SendRegistrationEmail(EmailRegisterMessageDto message);
}
