using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text.Encodings.Web;
using domis.api.Models;

namespace domis.api.Common;

public interface ICustomEmailSender<TUser> where TUser: UserEntity, new()/* : IEmailSender*/
{
    Task SendPasswordResetCodeAsync(UserEntity user, string email, string resetCode);
    Task SendConfirmationLinkAsync(UserEntity user, string toEmail, string confirmationLink);
}

public class CustomEmailSender(IConfiguration configuration, ILogger<CustomEmailSender> logger, ISendGridClient sendGridClient) 
    : ICustomEmailSender<UserEntity>
{

    public async Task SendConfirmationLinkAsync(UserEntity user, string toEmail, string confirmationLink)
    {
        var subject = "Potvrdite svoju email adresu";

        // Create a detailed and formatted message in Serbian
        var message = $@"
            <h1>Potvrda email adrese</h1>
            <p>Poštovani {user.UserName},</p>
            <p>Hvala što ste se registrovali! Molimo vas da potvrdite svoju email adresu klikom na link ispod:</p>
            <p>
                <a href='{HtmlEncoder.Default.Encode(confirmationLink)}' style='color: #007BFF; text-decoration: none;'>
                    Potvrdi email adresu
                </a>
            </p>
            <p>Ukoliko niste tražili ovu akciju, možete slobodno ignorisati ovu poruku.</p>
            <p>Hvala!</p>
        ";

        await SendEmailAsync(toEmail, subject, message);
    }

    public async Task SendPasswordResetCodeAsync(UserEntity user, string email, string resetCode)
    {
        var frontendBaseUrl = "http://localhost:5173";

        var subject = "Zahtev za promenu lozinke";

        var resetLink = $"{frontendBaseUrl}/promena-sifre?email={email}&code={resetCode}";

        //var message = resetCode;

        var message = $@"
            <h1>Zahtev za promenu lozinke</h1>
            <p>Poštovani {user.UserName},</p>
            <p>Kliknite na sledeći link kako biste promenili lozinku:</p>
            <a href='{resetLink}' style='color: #1a73e8;'>Promena lozinke</a>
            <p>Ukoliko niste zahtevali promenu lozinke, možete slobodno ignorisati ovu poruku.</p>
            <p>Hvala!</p>";

        await SendEmailAsync(email, subject, message);
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(configuration["Sendgrid:From"], configuration["Sendgrid:Name"]),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        var response = await sendGridClient.SendEmailAsync(msg);
        logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}