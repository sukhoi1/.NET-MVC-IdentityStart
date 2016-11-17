using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentityStart.Infrastructure;
using IdentityStart.Models;
using Microsoft.AspNet.Identity;
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser() { UserName = model.Name, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            return View(model);
        }

        public async Task<ActionResult> Delete(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] {"User Not Found"});
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(new CreateModel() { Id = user.Id, Email = user.Email, Name = user.UserName });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CreateModel viewModel)
        {
            AppUser user = null;
            if (viewModel != null)
            {
                 user = await UserManager.FindByIdAsync(viewModel.Id);
            }
            
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    user.Email = viewModel.Email;
                    IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                    if (!validEmail.Succeeded)
                    {
                        AddErrorsFromResult(validEmail);
                    }

                    IdentityResult validPass = null;
                    if (!string.IsNullOrWhiteSpace(viewModel.Password))
                    {
                        validPass = await UserManager.PasswordValidator.ValidateAsync(viewModel.Password);
                        if (validPass.Succeeded)
                        {
                            user.PasswordHash = UserManager.PasswordHasher.HashPassword(viewModel.Password);
                        }
                        else
                        {
                            AddErrorsFromResult(validEmail);
                        }
                    }

                    // validPass == null: a bit strange logic, needs to be tested thoroughly
                    if (validEmail.Succeeded && (validPass == null || !string.IsNullOrWhiteSpace(viewModel.Password) && validPass.Succeeded))
                    {
                        IdentityResult result = await UserManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrorsFromResult(validEmail);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User Not Found");
            }

            return View(viewModel);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}