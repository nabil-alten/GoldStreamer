using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainClasses
{
    public class QuestionGroup
    {
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages),
            ErrorMessageResourceName = "BasketNameLengthVal")]

        [RegularExpression("[A-Za-zأ-ي\\s]*", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "CharsOnly")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "QuestionGroupVal")]
        public string GroupName { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ZeroPrice")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "QuestionGroupVal")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "NumbersOnly")]
        public int GroupOrder { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
