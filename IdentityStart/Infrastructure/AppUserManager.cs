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
            return manager;
        }
    }
}