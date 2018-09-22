using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asopalav.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    [RoutePrefix("User")]
    [Route("{action}")]
    public class UserController : Controller
    {
        AsopalavDBEntities objAsopalavDBEntities = new AsopalavDBEntities();

        [Route("~/Admin/User/List")]
        public ActionResult UserList()
        {
            return View();
        }
    }
}