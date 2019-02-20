using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using BLL.DomainClasses;

namespace BLL.SecurityClasses
{
    public class ApplicationUser : IdentityUser
    {
        public int TraderId { get; set; }
      
        public bool IsActive { get; set; }
        public bool NeedReset { get; set; }
        public bool IsVerified { get; set; }
        public Guid Token { get; set; }
        public DateTime TokenCreationDate { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string DefaultPassword { get; set; }
        public bool IsLockedOut { get; set; }
        public int AccessFailedCount { get; set; }
        public bool MainUser { get; set; } 

    }
}
