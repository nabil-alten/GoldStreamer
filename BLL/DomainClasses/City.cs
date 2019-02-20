using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Resources;

namespace BLL.DomainClasses
{
    public class City
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

        public int GovernorateID { get; set; }
        public Governorate Governorate { get; set; }
        public virtual List<Region> Region { get; set; }
        [ForeignKey("City")]
        public List<Trader> Trader { get; set; }
    }
}