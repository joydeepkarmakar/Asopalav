using Asopalav.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asopalav.Controllers
{
    public class SilverwareController : Controller
    {
        AsopalavDBEntities objAsopalavDBEntities = new AsopalavDBEntities();
        DashboardModel objDashboardModel = new DashboardModel();
        // GET: Silverware
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("~/DinnerSet")]
        //[Route("~/Silverware/DinnerSet")]
        public ActionResult DinnerSet(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [HttpGet]
        [Route("~/PoojaSet")]
        public ActionResult PoojaSet(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [HttpGet]
        [Route("~/TeaSet")]
        public ActionResult TeaSet(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [HttpGet]
        [Route("~/LemonSet")]
        public ActionResult LemonSet(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [HttpGet]
        [Route("~/BedroomSet")]
        public ActionResult BedroomSet(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [HttpGet]
        [Route("~/FruitBowl")]
        public ActionResult FruitBowl(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }

        [HttpGet]
        [Route("~/KnifeForkSpoons")]
        public ActionResult KnifeForkSpoons(string page)
        {
            objDashboardModel.objGetProductsByProductType_Result = objAsopalavDBEntities.GetProductsByProductType(page, (string)Session["CurrentCurrency"] ?? "", (string)Session["DollarRate"] ?? "").ToList();
            objDashboardModel.ProductListPage = page;
            return View(objDashboardModel);
        }
    }
}