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


namespace GoldStreamer.Models.ViewModels
{
    public class ApplicationUserViewModel
    {
        public string ID { get; set; }
        public int TraderId { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public int typeFlg { get; set; } 
    }




}