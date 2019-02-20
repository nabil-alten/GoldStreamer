using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.DomainClasses;
namespace GoldStreamer.Models.ViewModels
{
    public class FAQViewModel
    {
        public int ID { get; set; } // Primary Key 
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public int? QuestionGroupId { get; set; }
        public string VideoLink { get; set; }
        public int ReadingCounter { get; set; }
        public bool IsActive { get; set; }

        public List<QuestionGroup> QuestionGroupList { get; set; }
        public List<Question> QuestionsList { get; set; } 
    }
} 