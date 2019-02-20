using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Resources;
namespace BLL.DomainClasses
{
    public class Region
    {
        [Required]
        public int ID { get; set; } // Primary Key 
        //[Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "CodeReq")]
        //[StringLength(5, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "codeValidation")]
        //[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "codeRegEx")]

        public string Code { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PrimReq")]
        //[StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "primeValidation")]
        //[RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "PrimeRegEx")]

        public string Name { get; set; }

        public int CityID { get; set; }
        public City City { get; set; }
        [ForeignKey("District")]
        public List<Trader> Trader { get; set; }
    }
}