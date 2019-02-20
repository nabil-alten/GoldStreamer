using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources;

namespace BLL.DomainClasses
{
    public class News
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinLengthThree")]
        //[RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "BasketNameNoNum")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [StringLength(100, MinimumLength = 20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinLengthTwenty")]
        public string Summary { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [StringLength(4000, MinimumLength = 20, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MinLengthTwenty")]
        public string Body { get; set; }

        public string Photo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd H:mm}")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }
        public NewsCategory NewsCategory { get; set; }
        public bool IsActive { get; set; }
        public bool IsLatest { get; set; }
        public string SourceName { get; set; }
        [RegularExpression(@"^((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_{}]*)#?(?:[\w]*))?)$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MailExpErr")]
        public string SourceUrl { get; set; }
        public int ViewCount { get; set; }

        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [Range(1, 99999, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "negativeVal")]
        public int Order { get; set; }
    }
}
