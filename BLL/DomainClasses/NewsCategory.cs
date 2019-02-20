using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resources;

namespace BLL.DomainClasses
{
    public class NewsCategory
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages),
            ErrorMessageResourceName = "BasketNameLengthVal")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "QuestionGroupVal")]
        [RegularExpression("[A-Za-zأ-ي\\s]*", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "CharsOnly")]
        public string CategoryName { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ZeroPrice")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "QuestionGroupVal")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "NumbersOnly")]
        public int CategoryOrder { get; set; }

        public int MainCategoryId { get; set; }

        
        public NewsMainCategory MainCategory { get; set; }

        [ForeignKey("CategoryId")]
        public List<News> News { get; set; }
    }
}
