using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asopalav.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    [RoutePrefix("Price")]
    [Route("{action}")]
    public class PriceController : Controller
    {
        [Route("~/Admin/Price")]
        public ActionResult Index()
        {
            Session["CurrentPage"] = "Prices";
            return View();
        }
    }
}