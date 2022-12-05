namespace Benkyoukai.Email.Contracts;

public record EmailRegisterMessageDto(
    string Subject,
    string Body,
    string Address);
