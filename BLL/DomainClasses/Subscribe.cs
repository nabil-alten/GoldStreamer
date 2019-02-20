using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;
using System;


namespace BLL.DomainClasses
{
    public class Subscribe
    { 
        [Required]
        public Guid ID { get; set; } // Primary Key
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "BasketNameLengthVal")]

       // [RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "BasketNameNoNum")]
        [RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "BasketNameNoNum")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.\+]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MailExpErr")]
        public string Email { get; set; }
        public bool IsSubscribe { get; set; }
    }
}