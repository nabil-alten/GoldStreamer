using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;
namespace BLL.DomainClasses
{
    public class Governorate
    {

        [Required]
        public int ID { get; set; } // Primary Key 
        //[Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "CodeReq")]
        //[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "codeRegEx")]
        //[StringLength(5, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "codeValidation")]
        public string Code { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PrimReq")]
        //[RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PrimeRegEx")]
        //[StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "primeValidation")]
        public string Name { get; set; }

        public virtual List<City> City { get; set; }
             [ForeignKey("Governorate")]
        public List<Trader> Trader { get; set; }
     
    }
}