using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Benkyoukai.Api.Services.Email;

public class EmailService
{
    public async Task<bool> SendEmailAsync(string subject, string body, string address = "rick6@ethereal.email")
    {
        // todo - send email
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("rick6@ethereal.email"));
        email.To.Add(MailboxAddress.Parse(address));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = body
        };

        using var smtp = new SmtpClient();
        smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("rick6@ethereal.email", "hAqsV9s8yQR41EVQcb");
        await smtp.SendAsync(email);
        smtp.Disconnect(true);

        return true;
    }
}
