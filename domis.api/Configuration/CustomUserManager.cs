using domis.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace domis.api.Configuration;

public class CustomUserManager<TUser>(IUserStore<TUser> store,
                         IOptions<IdentityOptions> optionsAccessor,
                         IPasswordHasher<TUser> passwordHasher,
                         IEnumerable<IUserValidator<TUser>> userValidators,
                         IEnumerable<IPasswordValidator<TUser>> passwordValidators,
                         ILookupNormalizer keyNormalizer,
                         IdentityErrorDescriber errors,
                         IServiceProvider services,
                         ILogger<UserManager<TUser>> logger) : UserManager<TUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) where TUser : UserEntity
{
    public override async Task<IdentityResult> CreateAsync(TUser user, string password)
    {
        var result = await base.CreateAsync(user, password);

        if (result.Succeeded)
        {
            //assign default role - 'User' to new users
            var defaultRole = Roles.User.RoleName();
            await AddToRoleAsync(user, defaultRole);
        }

        return result;
    }
}