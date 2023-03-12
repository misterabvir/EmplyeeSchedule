using EmployesWork.WebUI.Infrastructure;
using EmployesWork.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployesWork.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;
        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", "Не верный логин или пароль, попытайтесь еще раз");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }

}