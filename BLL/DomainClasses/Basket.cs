using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace BLL.DomainClasses
{
    public class Basket
    {
        public int ID { get; set; }
        public int BasketOwner { get; set; }
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "BasketNameVal")]
        [StringLength(50, MinimumLength = 2, ErrorMessageResourceType = typeof(Messages),
            ErrorMessageResourceName = "BasketNameLengthVal")]
        public string Name { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public List<BasketPrices> BasketPrices { get; set; }
        public List<BasketTraders> BasketTraders { get; set; }
    }
}