using Asopalav.Areas.Admin.Models;
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

        public ActionResult GetUserList()
        {
            var userList = objAsopalavDBEntities.UserProfileMasters.Where(x => x.IsActive == true).AsEnumerable().Select(x => new UserListModel
            {
                UserId = x.Login_Id,
                UserEmail = x.Primary_Email,
                UserFirstName = x.User_Fname,
                UserLastName = x.User_Lname,
                UserRoleName = x.RoleMaster.RoleName,
                UserMobile = x.Mobile,
                UserDOB = (x.User_DOB != null) ? x.User_DOB.Value.ToString("d/M/yyyy") : "NA",
                UserAnniversaryDate = (x.User_Anniversary != null) ? x.User_Anniversary.Value.ToString("d/M/yyyy") : "NA"
            }).ToList();
            return Json(new
            {
                data = userList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUserRole(long userId, string userRoleName, string userEmail)
        {
            //try
            //{
                UserProfileMaster userMaster = (from u in objAsopalavDBEntities.UserProfileMasters
                                                where u.Login_Id == userId && u.Primary_Email == userEmail
                                                select u).SingleOrDefault();

                userMaster.RoleMaster.RoleName = userRoleName;
                objAsopalavDBEntities.SaveChanges();
                return Json(new
                {
                    data = userMaster
                }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception)
            //{
            //}
        }
    }
}