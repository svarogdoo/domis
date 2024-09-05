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

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtp;
    private readonly IConfiguration _configuration;

    private readonly string _host;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;

    public EmailService(SmtpClient smtp, IConfiguration configuration)
    {
        _smtp = smtp;
        _configuration = configuration;

        // Load SMTP settings from configuration
        _host = _configuration["SmtpSettings:Host"] ?? "";
        _port = int.Parse(_configuration["SmtpSettings:Port"] ?? "0");
        _username = _configuration["SmtpSettings:Username"] ?? "";
        _password = _configuration["SmtpSettings:Password"] ?? "";
    }

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
            if (!_smtp.IsConnected)
            {
                await _smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
            }

            if (!_smtp.IsAuthenticated)
            {
                await _smtp.AuthenticateAsync(_username, _password);
            }

            string response = await _smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending email to {email}", to);
        }
        finally
        {
            await _smtp.DisconnectAsync(true);
        }
    }
}
