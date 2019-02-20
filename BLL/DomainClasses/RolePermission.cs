using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.DomainClasses
{
    public class RolePermission
    {
        [Required]
        public int ID { get; set; } // Primary Key 

        public Guid AspNetRolesID { get; set; }
        public int UserTypeID { get; set; }
    }
}