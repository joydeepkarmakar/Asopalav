using Asopalav.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Asopalav.Helpers;
using System.Configuration;

namespace Asopalav.Controllers
{
    public class HomeController : Controller
    {
        AsopalavDBEntities objAsopalavDBEntities = new AsopalavDBEntities();

        public ActionResult Index()
        {
            #region Dollar Silver Rate Section
            GetDollarSilverRate_Result objGetDollarSilverRate_Result = new GetDollarSilverRate_Result();
            objGetDollarSilverRate_Result = objAsopalavDBEntities.GetDollarSilverRate().FirstOrDefault();
            if (objGetDollarSilverRate_Result != null)
            {
                Session["DollarRate"] = objGetDollarSilverRate_Result.DollarRate;
                Session["SilverRate"] = objGetDollarSilverRate_Result.SilverRate;
            }
            #endregion

            #region Last Added Product Section
            DashboardModel objDashboardModel = new DashboardModel();
            objDashboardModel.objGetLastAddedProducts_Result = objAsopalavDBEntities.GetLastAddedProducts().ToList();
            #endregion

            return View(objDashboardModel);
        }

        [Route("~/About")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Route("~/Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        [Route("~/Contact")]
        public ActionResult Feedback(FeedbackMaster objFeedbackMaster)
        {
            string errorMsg = string.Empty;
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorMsg = "Not valid model";
                return Json(new { IsSuccess = false, errorMsg }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                objFeedbackMaster.FeedbackDate = DateTime.Now;
                objAsopalavDBEntities.FeedbackMasters.Add(objFeedbackMaster);
                objAsopalavDBEntities.SaveChanges();

                if (!SendAcknowledgementMail(objFeedbackMaster.EmailID))
                    return Json(new { IsSuccess = false, errorMsg = TempData["SendAcknowledgementMailMsg"] }, JsonRequestBehavior.AllowGet);

                if(!SendMailToAdmin(objFeedbackMaster))
                    return Json(new { IsSuccess = false, errorMsg = TempData["SendMailToAdminMsg"] }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        private bool SendAcknowledgementMail(string emailId)
        {
            bool IsSendMail = false;
            var host = System.Web.HttpContext.Current.Request.Url.OriginalString.Replace(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "");
            string imageName = host + "/Content/images/logo@2x.png";

            //Mail For Contacted User
            string msg = "<p><img src='" + imageName + "' alt='LogoName' border='0' width='95px' height='77px'/></p>" +
                        "<p><em>Dear Visitor</em></p>" +
                        "<p><em>Thank you for visiting our site and your valued feedback.</em></p>" +
                        "<p><em>Your sincerely</em></p>" +
                        "<p><em> - Asopalav Team</em></p>";
            try
            {
                MailHelper.SendMailMessage(ConfigurationManager.AppSettings["MailFrom"], emailId, string.Empty, string.Empty, "Acknowledgement From Asopalav Jewellers", msg);
                IsSendMail = true;
            }
            catch (Exception ex)
            {
                TempData["SendAcknowledgementMailMsg"] = ex.InnerException;
            }
            return IsSendMail;
        }

        private bool SendMailToAdmin(FeedbackMaster objFeedbackMaster)
        {
            bool IsSendMail = false;
            var host = System.Web.HttpContext.Current.Request.Url.OriginalString.Replace(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "");
            string imageName = host + "/Content/images/logo@2x.png";

            //Mail to Admin
            string msg = "<p><img src='" + imageName + "' alt='LogoName' border='0' width='95px' height='77px'/></p>" +
                                      "<p><em><strong>Subject:</strong> " + objFeedbackMaster.FeedbackSubject + "</em></p>" +
                                      "<p><em><strong>Queries:</strong> " + objFeedbackMaster.FeedbackMessage + "</em></p>" +
                                      "<p><em> - Asopalav Jewellers</em></p>";
            try
            {
                MailHelper.SendMailMessage(ConfigurationManager.AppSettings["MailFrom"], ConfigurationManager.AppSettings["MailFrom"], string.Empty, string.Empty, "Feedback - Asopalav Jewellers", msg);
                IsSendMail = true;
            }
            catch (Exception ex)
            {
                TempData["SendMailToAdminMsg"] = ex.InnerException;
            }

            return IsSendMail;
        }
    }
}