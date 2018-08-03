using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asopalav.Controllers
{
    public class CartDetailsController : Controller
    {
        [Route("Cart/{id}")]
        public ActionResult Index()
        {
            return View();
        }
    }
}