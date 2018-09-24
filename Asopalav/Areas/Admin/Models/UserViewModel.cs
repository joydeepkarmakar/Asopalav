using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asopalav.Areas.Admin.Models
{
    public class UserListModel
    {
        public long UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserFullName
        {
            get
            {
                return String.Format("{0} {1}", this.UserFirstName, this.UserLastName);
            }
        }
        public string UserRoleName { get; set; }
        public string UserMobile { get; set; }
        public string UserDOB { get; set; }
        public string UserAnniversaryDate { get; set; }
    }
}