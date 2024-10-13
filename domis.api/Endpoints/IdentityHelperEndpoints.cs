using domis.api.Extensions;
using domis.api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace domis.api.Endpoints;

public static class IdentityHelperEndpoints
{
    public static void RegisterIdentityHelperEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/identity").WithTags("Identity Helper");

        group.MapPost("/forgotPassword", async Task<Results<Ok, ValidationProblem>>
            ([FromBody] ForgotPasswordRequest resetRequest, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<CustomUserManager<UserEntity>>();
            var emailSender = sp.GetRequiredService<IEmailSender>();

            var user = await userManager.FindByEmailAsync(resetRequest.Email);

            if (user is not null && await userManager.IsEmailConfirmedAsync(user))
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                await emailSender.SendPasswordResetCodeAsync(user, resetRequest.Email, HtmlEncoder.Default.Encode(code));
            }

            // Return 200 OK regardless of whether the email is valid or not
            return TypedResults.Ok();
        });
    }
}
