using Asopalav.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

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
        public ActionResult Feedback(FeedbackModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errorMsg = "Not valid model";
                return Json(new { IsSuccess = false, errorMsg }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { IsSuccess = true }, JsonRequestBehavior.AllowGet);
        }
    }
}