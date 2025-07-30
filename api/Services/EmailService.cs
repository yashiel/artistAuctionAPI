using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;

namespace api.Services;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public void SendEmail(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Artist Auction App", _emailSettings.SmtpUser));
        message.To.Add(new MailboxAddress("User", toEmail));
        message.Subject = subject;
        var textPart = new TextPart("plain")
        {
            Text = body
        };
        message.Body = textPart;
        using (var client = new SmtpClient())
        {
            client.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort,
                SecureSocketOptions.StartTls);
            client.Authenticate(_emailSettings.SmtpUser, _emailSettings.SmtpPassword);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}