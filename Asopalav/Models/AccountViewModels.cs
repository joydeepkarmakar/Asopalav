using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asopalav.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "The Email is required")]
        [DisplayName("Email")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid Email")]
        public string Primary_Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*~]).\S{6,16}$", ErrorMessage = "Invalid Password. Min. 6 & Max. 16 characters.Password must contain at least one upper case letter,one lower case letter, one special character and one digit.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        public string User_Fname { get; set; }

        [DisplayName("Middle Name")]
        public string User_Mname { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        public string User_Lname { get; set; }

        [DisplayName("Alternate Email")]
        public string Secondary_Email { get; set; }

        [Required(ErrorMessage = " Mobile no. is required")]
        [DisplayName("Mobile No.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile")]
        public string Mobile { get; set; }

        [DisplayName("Alternate Mobile No.")]
        public string Alternate_Mobile { get; set; }

        [Required(ErrorMessage = " Gender is required")]
        [DisplayName("Gender ")]
        public string Gender { get; set; }
        public List<System.Web.Mvc.SelectListItem> GenderList { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = " {0} is required")]
        [CustomDate(ErrorMessage = "Invalid date")]
        public Nullable<System.DateTime> User_DOB { get; set; }

        [Display(Name = "Anniversary Date")]
        [CustomDate(ErrorMessage = "Invalid date")]
        public Nullable<System.DateTime> User_Anniversary { get; set; }

        public class CustomDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                try
                {
                    DateTime d = Convert.ToDateTime(value);
                    return d < DateTime.Now.AddDays(-1);
                }
                catch { return false; }
            }
        }
    }

    public class FeedbackModel
    {
        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid Email")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Mobile no. is required")]
        [DisplayName("Mobile No.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [DisplayName("Subject")]
        public string FeedbackSubject { get; set; }

        [Required(ErrorMessage = "Queries are required")]
        [DisplayName("Queries")]
        public string FeedbackMessage { get; set; }
    }
}