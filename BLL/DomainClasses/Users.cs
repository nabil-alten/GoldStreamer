using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainClasses
{
    public class Users
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [RegularExpression("[a-zA-Z0-9._^%$#!~@,-]+", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "EnglishCharactersandNumbersOnly")]
        [StringLength(20, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MinLengthThree")]
        public string UserName { get; set; }
          [Required]
        public string Password { get; set; }
        public int TraderId { get; set; }
        public bool IsActive { get; set; }
        public bool NeedReset { get; set; }
        public bool IsVerified { get; set; }
        public Guid Token { get; set; }
        public string Email { get; set; }


    }
}
