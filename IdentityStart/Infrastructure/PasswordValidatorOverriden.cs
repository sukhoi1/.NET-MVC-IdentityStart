using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IdentityStart.Infrastructure
{
    public class PasswordValidatorOverriden : PasswordValidator
    {
        public PasswordValidatorOverriden(string contains, PasswordValidator passwordValidator) : base()
        {
            this.Contains = contains;
            this.RequireDigit = passwordValidator.RequireDigit;
            this.RequireLowercase = passwordValidator.RequireLowercase;
            this.RequireNonLetterOrDigit = passwordValidator.RequireNonLetterOrDigit;
            this.RequireUppercase = passwordValidator.RequireUppercase;
            this.RequiredLength = passwordValidator.RequiredLength;
        }

        public string Contains { get; set; }

        public override async Task<IdentityResult> ValidateAsync(string pass)
        {
            IdentityResult result = await base.ValidateAsync(pass);
            if (pass.Contains(this.Contains))
            {
                var errors = result.Errors.ToList();
                errors.Add("Passwords can not contain numeric sequences");
                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}