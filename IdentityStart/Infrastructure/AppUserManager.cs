using System.Web.Configuration;
using IdentityStart.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace IdentityStart.Infrastructure
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
        }

        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options,
            IOwinContext context)
        {
            var db = context.Get<AppIdentityDbContext>();
            var manager = new AppUserManager(new UserStore<AppUser>(db));
            // A better way to get UserManager from OWIN:
            // var manager = context.GetUserManager<AppUserManager>();

            var validator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            if (bool.Parse(WebConfigurationManager.AppSettings["enableStrictPasswordValidation"]))
            {
                manager.PasswordValidator = validator;
            }

            if (bool.Parse(WebConfigurationManager.AppSettings["enableCustomPasswordValidation"]))
            {
                manager.PasswordValidator = new PasswordValidatorOverriden("12345", validator);
            }

            return manager;
        }
    }
}