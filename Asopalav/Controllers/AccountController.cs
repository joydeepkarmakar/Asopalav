using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asopalav.Models;
using DataAccessLayer;


namespace Asopalav.Controllers
{
    public class AccountController : Controller
    {
        AsopalavDBEntities objAsopalavDBEntities = new AsopalavDBEntities();

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                ValidateUserAndMenu_Result objValidateUserAndMenu_Result = new ValidateUserAndMenu_Result();
                objValidateUserAndMenu_Result = objAsopalavDBEntities.ValidateUserAndMenu(model.Email, model.Password).FirstOrDefault();

                if (objValidateUserAndMenu_Result.IsLoginValid)
                {
                    Session["IsLoginValid"] = true;
                    Session["UserFullName"] = objValidateUserAndMenu_Result.UserFullName;

                    if (objValidateUserAndMenu_Result.RoleName == "Admin")
                        return RedirectToAction("Index", "Product", new { area = "Admin" });
                    else
                        return RedirectToAction("Index", "Home");
                }

                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewData["Gender"] = GetGenderList();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionName("Register")]
        public ActionResult NewRegistration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                int result = objAsopalavDBEntities.AddUser(model.Primary_Email, model.Password, model.User_Fname, model.User_Mname, model.User_Lname, model.Secondary_Email, model.Mobile, model.Alternate_Mobile, model.Gender, model.User_DOB, model.User_Anniversary);

                if (result == -1)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewData["Gender"] = GetGenderList();
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Remove("IsLoginValid");
            Session.Remove("UserFullName");

            //Disable back button In all browsers.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        #region Helpers
        private List<SelectListItem> GetGenderList()
        {
            List<SelectListItem> listG = new List<SelectListItem>();
            listG.Add(new SelectListItem() { Value = "M", Text = "Male" });
            listG.Add(new SelectListItem() { Value = "F", Text = "Female" });
            listG.Add(new SelectListItem() { Value = "O", Text = "Other" });
            return listG;
        }
        #endregion
    }
}