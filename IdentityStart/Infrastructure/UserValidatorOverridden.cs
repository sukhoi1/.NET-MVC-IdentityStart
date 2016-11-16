using System.Linq;
using System.Threading.Tasks;
using IdentityStart.Models;
using Microsoft.AspNet.Identity;

namespace IdentityStart.Infrastructure
{
    public class UserValidatorOverridden : UserValidator<AppUser>
    {
        public UserValidatorOverridden(string emailDomain, AppUserManager userManager) : base(userManager)
        {
            _emailDomain = emailDomain;
        }

        private readonly string _emailDomain;

        public override async Task<IdentityResult> ValidateAsync(AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);
            var errors = result.Errors.ToList();

            // Multiple User validation condition example:
            if (!user.Email.ToLower().EndsWith(_emailDomain))
            {
                errors.Add($"Only '{_emailDomain}' email domains are allowed");
                result = new IdentityResult(errors);
            }

            //if (user.Email == user.PasswordHash) // But NOT user.Password!
            //{
            //    errors.Add("Password and username can not match");
            //}

            if (errors.Count > 0)
            {
                return new IdentityResult(errors); //result.Succeeded is readonly property
            }

            return result;
        }
    }
}