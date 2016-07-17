
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using Mvc.Identity.BLL;
using Mvc.Identity.DAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Identity2Study.Models;
using Mvc.Identity.Common;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;




namespace Identity2Study.Controllers
{
   
    [Authorize(Roles="Admin")]
    public class AccountController : Controller
    {/*
        public AccountController()            
        {
        }*/

        ApplicationDbContext context = new ApplicationDbContext();

         
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))          

        {
        }

        //static UserManager<ApplicationUser>  UserManagerNew = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
       
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        
        public UserManager<ApplicationUser> UserManager { get; private set; }

        
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        
        //public  ApplicationUserManager _userManager;
        /*
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        */
     
        private ApplicationSignInManager _signInManager;
       
        
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //markSureEmail: false 表示登陆不需要到邮箱去确认
            var result = await SignInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, shouldLockout: false, markSureEmail: false);
            //var result = await SignInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, shouldLockout: false, markSureEmail: true);
            //var result = await SignInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, shouldLockout: false);
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case AppSignInStatus.Success:
                //case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case AppSignInStatus.LockedOut:
                //case SignInStatus.LockedOut:
                    return View("Lockout");
                case AppSignInStatus.RequiresVerification:
                //case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case AppSignInStatus.NotSureEmail:
                    return View("ConfirmeEmail");
                case AppSignInStatus.Failure:
                //case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //以下为辅助方法
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public ActionResult Index()
        {
            var users = context.Users.ToList();
            return View(users);
        }
/*

        public ActionResult Index()
        {
            return View();
        }*/

        public ActionResult Delete(string UserName)
        {
            var thisUser = context.Users.Where(r => r.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Users.Remove(thisUser);
            context.SaveChanges();

            var users = context.Users.ToList();
            //return View(users);

            return RedirectToAction("Index",users);

            //return View();
        }

        /*
        [HttpPost]
        public ActionResult Index(string UserName)
        {
            var thisUser = context.Users.Where(r => r.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Users.Remove(thisUser);
            context.SaveChanges();

            var users = context.Users.ToList();
            //return View(users);

            return RedirectToAction("Index",users);

            //return View();
        }
        */
        public ActionResult Edit(string UserName)
        {
            var thisUser = context.Users.Where(r => r.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            //context.Users.Remove(thisUser);
            //context.SaveChanges();

            //var users = context.Users.ToList();
            return View(thisUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser user)
        //public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityUser user)
        {

            try
            {
                context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
          
        }

        /*
        public ActionResult Delete(string UserName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }*/


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = new ApplicationUser() { UserName = model.UserName };
                var user = new ApplicationUser() { UserName = model.Name, Email = model.Email, EmailConfirmed = false };
                //var user = new ApplicationUser() { UserName = model.Name, Email = model.Email, EmailConfirmed = true };
                //var user = new ApplicationUser() { UserName = model.Name, Email = model.Email };
                
                var UserManager1 = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); 
                var result = await UserManager1.CreateAsync(user, model.Password);

                //var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await UserManager1.GenerateEmailConfirmationTokenAsync(user.Id);
                    //var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmeEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager1.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    //await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    return View("ConfirmeEmail");

                    //await SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    //AddErrors(result);
                }
                AddErrors(result);

            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }



        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //用户邮件确认
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmeEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var UserManager1 = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var result = await UserManager1.ConfirmEmailAsync(userId, code);
            //var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "SureEmail" : "Error");
        }


	}
}