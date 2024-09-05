using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Serilog;

namespace domis.api.Services;

public interface IEmailService
{
    public Task SendEmail(string to, string subject, string body);
}

public class EmailService(SmtpClient smtp) : IEmailService
{
    private readonly string _host = "smtp.gmail.com";
    private readonly string _username = "rluka996@gmail.com";
    private readonly string _password = "...";
    private readonly int _port = 587; // Port commonly used for TLS

    public async Task SendEmail(string to, string subject, string body)
    {
        var email = new MimeMessage
        {
            From = { new MailboxAddress("Domis Enterijeri MailKit", _username) },
            To = { MailboxAddress.Parse(to) },
            Subject = subject,
            Body = new TextPart(TextFormat.Text) //Html
            {
                Text = body
            }
        };

        try
        {
            if (!smtp.IsConnected)
            {
                await smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
            }

            if (!smtp.IsAuthenticated)
            {
                await smtp.AuthenticateAsync(_username, _password);
            }

            string response = await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending email to {email}", to);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}
