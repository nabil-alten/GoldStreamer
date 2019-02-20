using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace BLL.DomainClasses
{
    public class NewsMainCategory
    {
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages),
            ErrorMessageResourceName = "BasketNameLengthVal")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "QuestionGroupVal")]
        [RegularExpression("[A-Za-zأ-ي\\s]*", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "CharsOnly")]
        public string MainCategoryName { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ZeroPrice")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "NumbersOnly")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "QuestionGroupVal")]
        public int MainCategoryOrder { get; set; }

        [ForeignKey("MainCategoryId")]
        public List<NewsCategory> NewsCategory { get; set; }
        
    }
}
