using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace BLL.DomainClasses
{
  public  class Feedback
    {

        [Required]
        public int ID { get; set; } // Primary Key 


        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Mintwo")]
        [RegularExpression("^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z ]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "LettersOnly")]
  
      public string Name { get; set; }

       [RegularExpression("^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z0-9 ]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z0-9 ]*$", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "LettersAndNumbersOnly")]

        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Mintwo")]
        public string Subject { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "MailExpErr")]
        public string Email { get; set; }


        
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [StringLength(250, MinimumLength = 2, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Mintwo")]
        public string Comment { get; set; }
    }
}
