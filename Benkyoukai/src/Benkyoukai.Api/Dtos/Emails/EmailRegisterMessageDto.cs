namespace Benkyoukai.Api.Dtos.Emails;

public record EmailRegisterMessageDto(
    string Subject,
    string Body,
    string Address);
