using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;


namespace BLL.DomainClasses
{
    public class Question
    {
        [Required]
        public int ID { get; set; } // Primary Key 


        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [StringLength(250, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Mintwo")]

        public string QuestionText { get; set; }
        
        
        //[Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
       [MaxLength]
        public string Answer { get; set; }


        //[Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        public int? QuestionGroupId { get; set; }
          [RegularExpression(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MailExpErr")]
        public string VideoLink { get; set; }

        [Required]

        public int ReadingCounter { get; set; }
        [NotMapped]
       // [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        public string QuestionGroup { get; set; }
       
        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "BasketNameLengthVal")]
        [RegularExpression(@"^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "BasketNameNoNum")]

        public string Name { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.\+]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MailExpErr")]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsReplied { get; set; }
        public bool IsUserQuestion { get; set; }
        public bool IsRepeated { get; set; }
        }
}
