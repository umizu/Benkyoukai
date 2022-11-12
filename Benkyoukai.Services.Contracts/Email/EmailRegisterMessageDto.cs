namespace Benkyoukai.Services.Contracts.Email;

public record EmailRegisterMessageDto(
    string Subject,
    string Body,
    string Address);
