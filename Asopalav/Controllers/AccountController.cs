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
        [Route("~/Login")]
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
        [Route("~/Login")]
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
                    Session["UserFirstName"] = objValidateUserAndMenu_Result.UserFirstName;
                    Session["PrimaryEmail"] = model.Email;
                    Session["UserRole"] = objValidateUserAndMenu_Result.RoleName;

                    if (objValidateUserAndMenu_Result.RoleName == "Admin" || objValidateUserAndMenu_Result.RoleName == "SuperAdmin")
                        return RedirectToAction("ProductList", "Product", new { area = "Admin" });
                    else
                        return RedirectToAction("Index", "Home");
                }

                else
                {
                    TempData["IsLogin"] = "false";
                    TempData["LoginMsg"] = "Invalid login attempt.";
                }
            }
            catch (Exception ex)
            {
                TempData["IsLogin"] = "false";
                if (ex.InnerException != null)
                {
                    TempData["LoginMsg"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["LoginMsg"] = ex.Message;
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("~/Register")]
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
        [Route("~/Register")]
        public ActionResult NewRegistration(RegisterViewModel model)
        {
            string errorMessage = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    int result = objAsopalavDBEntities.AddUser(model.Primary_Email, model.Password, model.User_Fname, model.User_Mname, model.User_Lname, model.Secondary_Email, model.Mobile, model.Alternate_Mobile, model.Gender, model.User_DOB, model.User_Anniversary);

                    if (result == -1)
                    {
                        Session["IsLoginValid"] = true;
                        Session["UserFirstName"] = model.User_Fname; //model.User_Fname + ' ' + model.User_Mname ?? ' ' + model.User_Lname;
                        return RedirectToAction("Index", "Home");
                    }

                    TempData["isNewRegistration"] = "true";
                    TempData["NewRegistrationMsg"] = "You have registered successfully.";
                }
                else
                {
                    foreach (var item in ModelState.Keys)
                    {
                        if (ModelState[item].Errors.Count > 0)
                        {
                            errorMessage += ModelState[item].Errors.ToList()[0].ErrorMessage;
                            TempData["isNewRegistration"] = "false";
                            TempData["NewRegistrationMsg"] = errorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["isNewRegistration"] = "false";
                if (ex.InnerException != null)
                {
                    TempData["NewRegistrationMsg"] = ex.InnerException.Message;
                }
                else
                {
                    TempData["NewRegistrationMsg"] = ex.Message;
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
            Session.Remove("UserFirstName");

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