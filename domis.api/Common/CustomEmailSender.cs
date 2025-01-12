using System.Text;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text.Encodings.Web;
using domis.api.Models;
using domis.api.DTOs.Order;
using domis.api.Models.Entities;

namespace domis.api.Common;

public interface ICustomEmailSender<TUser> where TUser: UserEntity, new()
{
    Task SendPasswordResetCodeAsync(UserEntity user, string email, string resetCode);
    Task SendConfirmationLinkAsync(UserEntity user, string toEmail, string confirmationLink);
    Task SendOrderConfirmationAsync(string email, OrderConfirmationDto order);
    Task SendOrderConfirmationInternallyAsync(string userEmail, string role, OrderConfirmationDto order);
}

public class CustomEmailSender(ILogger<CustomEmailSender> logger, ISendGridClient sendGridClient) 
    : ICustomEmailSender<UserEntity>
{

    public async Task SendConfirmationLinkAsync(UserEntity user, string toEmail, string confirmationLink)
    {
        const string subject = "Potvrdite svoju email adresu";

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
        const string frontendBaseUrl = "http://localhost:5173";

        const string subject = "Zahtev za promenu lozinke";

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
        var subject = $"Domis Enterijeri - Potvrda narudžbine #{order.OrderId}";

        var shippingAddress = $"{order.InvoiceAddress?.Address}";
        if (!string.IsNullOrWhiteSpace(order.InvoiceAddress?.Apartment))
            shippingAddress += $", {order.InvoiceAddress.Apartment}";
        if (!string.IsNullOrWhiteSpace(order.InvoiceAddress?.City))
            shippingAddress += $", {order.InvoiceAddress.City}";
        if (!string.IsNullOrWhiteSpace(order.InvoiceAddress?.PostalCode))
            shippingAddress += $", {order.InvoiceAddress.PostalCode}";

        var message = $@"
            <div style='font-family: Arial, sans-serif; color: #333;'>
                <h1 style='color: #4CAF50;'>Hvala vam na narudžbini #{order.OrderId}</h1>
                <p>Ovo su detalji vaše narudžbine:</p>
        
                <h2 style='border-bottom: 1px solid #ddd; padding-bottom: 5px;'>Stavke narudžbine</h2>
                <ul style='list-style-type: none; padding: 0;'>
                    {string.Join("", order.OrderItems.Select(item => $@"
                        <li style='margin-bottom: 10px;'>
                            <strong>{item.ProductName}:</strong> {item.ProductPrice} RSD 
                            <span style='font-size: 0.9em;'>(Količina: {item.Quantity})</span>
                        </li>
                    "))}
                </ul>

                <p style='font-weight: bold;'>Ukupna cena: {order.TotalPrice?.ToString("F2")} RSD</p>
        
                <h2 style='border-bottom: 1px solid #ddd; padding-bottom: 5px;'>Detalji isporuke</h2>
                <p><strong>Ime i prezime:</strong> {order.InvoiceAddress?.FirstName} {order.InvoiceAddress?.LastName}</p>
                <p><strong>Adresa:</strong> {shippingAddress}</p>
                <p><strong>Telefon:</strong> +381 {order.InvoiceAddress?.PhoneNumber}</p>
        
                <p style='margin-top: 20px;'>Ukoliko imate bilo kakva pitanja, slobodno nas kontaktirajte.</p>
            
                <a href='https://www.domisenterijeri.com' style='color: #8f1410; text-decoration: underline; margin-top: 20px;'>Posetite našu stranicu</a>
            
                <p style='font-size: 1.4em; color: #8f1410; margin-top: 10px;'>Pozdrav, Vaši Domis Enterijeri</p>
            </div>
        ";

        await SendEmailAsync(email, subject, message);
    }

    public async Task SendOrderConfirmationInternallyAsync(string userEmail, string role, OrderConfirmationDto order)
    {
        var subject = $"Domis Enterijeri - Potvrda narudžbine #{order.OrderId}";
        var message = $"Potvrda narudžbine #{order.OrderId}";
        
        var csvData = GenerateCsvAttachment(order, userEmail, role, out var attachmentFileName);

        //TODO: replace with domis internal email
        await SendEmailAsync("lukardvn@gmail.com", subject, message, csvData, attachmentFileName);
    }
    
    private static byte[] GenerateCsvAttachment(OrderConfirmationDto order, string userEmail, string role, out string attachmentFileName)
    {        
        var csv = new StringBuilder();

        var orderId = order.OrderId;
        var createdAtBelgrade = TimeZoneInfo.ConvertTimeFromUtc(order.CreatedAt, DateTimeHelper.BelgradeTimeZone).ToString("yyyy-MM-dd-HH-mm-ss");

        foreach (var item in order.OrderItems)
        {
            var quantity = item.UnitsQuantity;
            var sku = item.Sku;
            var price = item.ProductPrice;
            
            //TODO: check values
            csv.AppendLine($"{orderId},{role},x,{userEmail},{userEmail},{orderId},{createdAtBelgrade},{sku},{price},{quantity},0,0");
        }
        
        var csvData = Encoding.UTF8.GetBytes(csv.ToString());
        attachmentFileName = $"Profaktura-{createdAtBelgrade}-{Guid.NewGuid()}.csv";
        return csvData;
    }

    private async Task SendEmailAsync(string toEmail, string subject, string message,
        byte[]? attachmentData = null, string? attachmentFileName = null)
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

        if (attachmentData != null && !string.IsNullOrEmpty(attachmentFileName))
            msg.AddAttachment(attachmentFileName, Convert.ToBase64String(attachmentData));
        
        var response = await sendGridClient.SendEmailAsync(msg);
        logger.LogInformation(response is { IsSuccessStatusCode: true }
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}