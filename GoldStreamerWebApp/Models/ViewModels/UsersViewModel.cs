using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources;
using DAL;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using System.Web.Mvc;


namespace GoldStreamerWebApp.Models.ViewModels
{
    public class UsersViewModel
    {

        [Required]
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [RegularExpression("[a-zA-Z0-9._^%$#!~@,-]+", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "EnglishCharactersandNumbersOnly")]
        [StringLength(20, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MinLengthThree")]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MinLengthSix")]
        public string CurrPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "MinLengthSix")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "Required")]
        [System.Web.Mvc.Compare("Password", ErrorMessageResourceType = typeof(Resources.Messages), ErrorMessageResourceName = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
        public int TraderId { get; set; }
        public bool IsActive { get; set; }
        public bool NeedReset { get; set; }
    }




}