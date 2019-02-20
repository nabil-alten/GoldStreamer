using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BLL.DomainClasses;
using Resources;

namespace GoldStreamerWebApp.Models
{
    public class ManageUserViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MinLengthSix")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        public string UserName { get; set; }

    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [RegularExpression("[a-zA-Z0-9._]+", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "EnglishCharactersandNumbersOnly")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

      [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public bool Resend { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class TraderRegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [StringLength(20, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MinLengthThree")]
        [RegularExpression("[a-zA-Z0-9._]+", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "EnglishCharactersandNumbersOnly")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MinLengthSix")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [System.Web.Mvc.Compare("Password", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"TraderNameVal")]
        [RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "BasketNameNoNum")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "BasketNameLengthVal")]
        public string Name { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"TraderNameVal")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "BasketNameLengthVal")]
        [RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "BasketNameNoNum")]
        public string Family { get; set; }
        public bool Gender { get; set; }
        [RegularExpression(@"^([1-9]{0,1}[0-9]{0,100})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "phoneVal")]
        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "TraderNameVal")]
        [StringLength(11, MinimumLength = 6, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PhoneLengthVal")]
        public string Phone { get; set; }

        [RegularExpression(@"^([1-9]{0,1}[0-9]{0,100})$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "phoneVal")]
        [StringLength(15, MinimumLength = 11, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MobileLengthVal")]
        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")] //"mobileReq")]
        public string Mobile { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.\+]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MailExpErr")]
        public string Email { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.\+]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MailExpErr")]
        [System.Web.Mvc.Compare("Email", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ConfirmMailVal")]
        public string ReEmail { get; set; }

        public int? Governorate { get; set; }
        public int? City { get; set; }
        public int? District { get; set; }
        public int TypeFlag { get; set; }

        public List<Governorate>  Governorates { get; set; }
        public List<City> Cities { get; set; }
        public List<Region> Districts { get; set; }
    }

    public class MessagesVM
    {
        public string Message { get; set; }
    }
}
