using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text.Encodings.Web;
using domis.api.Models;
using domis.api.DTOs.Order;

namespace domis.api.Common;

public interface ICustomEmailSender<TUser> where TUser: UserEntity, new()/* : IEmailSender*/
{
    Task SendPasswordResetCodeAsync(UserEntity user, string email, string resetCode);
    Task SendConfirmationLinkAsync(UserEntity user, string toEmail, string confirmationLink);
    Task SendOrderConfirmationAsync(string email, OrderConfirmationDto order);
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

    public async Task SendOrderConfirmationAsync(string email, OrderConfirmationDto order)
    {
        var subject = $"Potvrda narudžbine #{order.OrderId}";

        var message = $@"
            <h1>Hvala vam na narudžbini #{order.OrderId}</h1>
            <p>Ovo su detalji vaše narudžbine:</p>
        
            <h2>Stavke narudžbine</h2>
            <ul>
                {string.Join("", order.OrderItems.Select(item => $@"
                    <li>
                        Proizvod #{item.ProductId}: {item.ProductPrice} RSD 
                        (Količina: {item.Quantity})
                    </li>
                "))}
            </ul>

            <p><strong>Ukupna cena:</strong> {order.TotalPrice?.ToString("F2")} RSD</p>
        
            <h2>Detalji isporuke</h2>
            <p><strong>Ime i prezime:</strong> {order.Shipping?.FirstName} {order.Shipping?.LastName}</p>
            <p><strong>Adresa:</strong> {order.Shipping?.Address}, {order.Shipping?.Apartment}, {order.Shipping?.City}, {order.Shipping?.PostalCode}</p>
            <p><strong>Telefon:</strong> {order.Shipping?.PhoneNumber}</p>
        
            <p>Ukoliko imate bilo kakva pitanja, slobodno nas kontaktirajte.</p>
            <p>Pozdrav, Vaš tim</p>
        ";

        await SendEmailAsync(email, subject, message);
    }


    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var msg = new SendGridMessage()
        {
            From = new EmailAddress(Environment.GetEnvironmentVariable("SENDGRID_FROM") ?? "rdvn.luka@gmail.com", 
                                    Environment.GetEnvironmentVariable("SENDGRID_NAME") ?? "Domis Enterijeri"),
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