using System.Web;
using System.Web.Mvc;
using IdentityStart.Infrastructure;
using Microsoft.AspNet.Identity.Owin;

namespace IdentityStart.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }
    }
}