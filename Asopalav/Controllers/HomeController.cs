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
using Asopalav.GetDailyGoldRateServiceReference;
using Asopalav.GetDailySilverRateServiceReference;

namespace Asopalav.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        AsopalavDBEntities objAsopalavDBEntities = new AsopalavDBEntities();
        DashboardModel objDashboardModel = new DashboardModel();

        public ActionResult Index(string currentCurrency)
        {
            Session["CurrentPage"] = "Home";
            #region Dollar Silver Rate Section
            /*
            //Scrapped due to admin permission issue in sql server//
            GetDollarSilverRate_Result objGetDollarSilverRate_Result = new GetDollarSilverRate_Result();
            objGetDollarSilverRate_Result = objAsopalavDBEntities.GetDollarSilverRate().FirstOrDefault();
            if (objGetDollarSilverRate_Result != null)
            {
                Session["DollarRate"] = objGetDollarSilverRate_Result.DollarRate;
                Session["SilverRate"] = objGetDollarSilverRate_Result.SilverRate;
            }
            */

            Session["DollarRate"] = GetDollarToRupeeVal(ConfigurationManager.AppSettings["DollarToRupeeUrl"]) ?? "NA";
            //Session["SilverRate"] = GetSilverPrice(ConfigurationManager.AppSettings["SilverPriceUrl"]) ?? "NA";
            Session["GoldRate"] = GetGoldPrice();//"1,197.75";
            Session["SilverRate"] = GetSilverPrice();//"14.32";
            #endregion

            var isAjax = Request.IsAjaxRequest();

            #region Last Added Product Section
            Session["CurrentCurrency"] = currentCurrency;
            objDashboardModel.objGetLastAddedProducts_Result = objAsopalavDBEntities.GetLastAddedProducts((string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            #endregion

            return View(objDashboardModel);
        }

        public JsonResult AjaxGetLastAddedProducts(string currentCurrency)
        {
            return this.Json(new { flag = "" });
        }

        [Route("~/About")]
        public ActionResult About()
        {
            Session["CurrentPage"] = "About";
            return View();
        }

        [HttpGet]
        [Route("~/Antiques")]
        public ActionResult Antiques(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [Route("~/Personalize")]
        public ActionResult Personalize(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [Route("~/Corporate")]
        public ActionResult Corporate(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [Route("~/Kids")]
        public ActionResult Kids(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [Route("~/Festive")]
        public ActionResult Festive(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [Route("~/Jewelery")]
        public ActionResult Jewelery(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [Route("~/FineMetal")]
        public ActionResult FineMetal(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [Route("~/Contact")]
        public ActionResult Contact()
        {
            Session["CurrentPage"] = "Contact";
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

                if (!SendMailToAdmin(objFeedbackMaster))
                    return Json(new { IsSuccess = false, errorMsg = TempData["SendMailToAdminMsg"] }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSuggestion(string txtSearch)
        {
            List<string> itemList = (from p in objAsopalavDBEntities.ProductMasters where p.ProductName.ToLower().Contains(txtSearch.ToLower()) select p.ProductName)
                                    .Union
                                    (from pt in objAsopalavDBEntities.ProductTypeMasters
                                     where pt.ProductType.ToLower().Contains(txtSearch.ToLower())
                                     select pt.ProductType)
                                    .ToList();
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        [Route("~/Search")]
        public ActionResult SearchResult(string prefix)
        {
            objDashboardModel.objSearchProducts_Result = objAsopalavDBEntities.SearchProducts(prefix, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = "SearchResult";
            return View(objDashboardModel);
        }

        #region MailHelperMethods
        private bool SendAcknowledgementMail(string emailId)
        {
            bool IsSendMail = false;
            var host = System.Web.HttpContext.Current.Request.Url.OriginalString.Replace(System.Web.HttpContext.Current.Request.Url.PathAndQuery, "");
            string imageName = host + "/Content/images/logo@2x.gif";

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
            string imageName = host + "/Content/images/logo@2x.gif";

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
        #endregion

        #region Dollar INR Conversion
        private string GetDollarToRupeeVal(string url)
        {
            var jsonData = JsonHelper.ReturnJsonData(url);
            var dollarToRupeeVal = (from x in JsonHelper.DeserializeAndFlatten(jsonData) where x.Key == "results.USD_INR.val" select x.Value).FirstOrDefault();
            return dollarToRupeeVal.ToString();
        }

        private string GetSilverPrice(string url)
        {
            var jsonData = JsonHelper.ReturnJsonData(url);
            var silverPrice = (from x in JsonHelper.DeserializeAndFlatten(jsonData) where x.Key == "data.SELL_PRICE" select x.Value).FirstOrDefault();
            return silverPrice.ToString();
        }

        private string GetGoldPrice()
        {
            GetGoldPriceSoapClient client = new GetGoldPriceSoapClient();
            var a = client.GetCurrentGoldPrice(ConfigurationManager.AppSettings["GoldSilverDailyRateUid"], ConfigurationManager.AppSettings["GoldSilverDailyRatePwd"]);
            return a[0].ToString();
        }

        private string GetSilverPrice()
        {
            GetSilverPriceSoapClient client = new GetSilverPriceSoapClient();
            return client.GetCurrentSilverPrice(ConfigurationManager.AppSettings["GoldSilverDailyRateUid"], ConfigurationManager.AppSettings["GoldSilverDailyRatePwd"]);
        } 
        #endregion
    }
}