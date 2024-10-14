//using Microsoft.AspNetCore.Identity.UI.Services;
//using SendGrid;
//using SendGrid.Helpers.Mail;
//using System.Text.Encodings.Web;

//namespace domis.api.Common;

//public class EmailSender : IEmailSender
//{
//    private readonly ILogger _logger;
//    private readonly IConfiguration _configuration;
//    private readonly ISendGridClient _sendGridClient;

//    public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger, ISendGridClient sendGridClient)
//    {
//        _configuration = configuration;
//        _logger = logger;
//        _sendGridClient = sendGridClient;
//    }

//    public async Task SendEmailAsync(string toEmail, string subject, string message)
//    {
//        var msg = new SendGridMessage()
//        {
//            From = new EmailAddress(_configuration["Sendgrid:From"], _configuration["Sendgrid:Name"]),
//            Subject = subject,
//            PlainTextContent = message,
//            HtmlContent = message
//        };
//        msg.AddTo(new EmailAddress(toEmail));

//        var response = await _sendGridClient.SendEmailAsync(msg);
//        _logger.LogInformation(response.IsSuccessStatusCode
//                               ? $"Email to {toEmail} queued successfully!"
//                               : $"Failure Email to {toEmail}");
//    }

//    public async Task SendPasswordResetCodeAsync(string toEmail, string resetCode)
//    {
//        var subject = "Password Reset Code";
//        var message = $"Your password reset code is: {resetCode}";

//        await SendEmailAsync(toEmail, subject, message);
//    }

//    public async Task SendConfirmationLinkAsync(string toEmail, string confirmationLink)
//    {
//        var subject = "Confirm Your Email Address";
//        var message = $"Please confirm your email by clicking this link: {HtmlEncoder.Default.Encode(confirmationLink)}";

//        await SendEmailAsync(toEmail, subject, message);
//    }
//}