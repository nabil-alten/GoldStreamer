using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.SecurityClasses;
using System.ComponentModel.DataAnnotations;

namespace GoldStreamer.Models.ViewModels
{
    public class RolePermissionViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }

        [Required]
        public bool Selected { get; set ; }

    }
}