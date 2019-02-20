using BLL.DomainClasses;
using DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public class QuestionGroupService
    {
       private readonly UnitOfWork _uow;
       public QuestionGroupService()
       {
           _uow = new UnitOfWork();
       }

       public List<QuestionGroup> ListQuestionGroups(string sortOrder)
       {
          
           sortOrder = sortOrder ?? "";
           List<QuestionGroup> questionGroupLst = _uow.QuestionGroupRepo.GetAll();


           if (questionGroupLst.Count > 0)
           {
               switch (sortOrder)
               {
                   case "nameDsc":
                       questionGroupLst = questionGroupLst.OrderByDescending(b => b.GroupName).ToList();
                       break;
                   case "nameAsc":
                       questionGroupLst = questionGroupLst.OrderBy(b => b.GroupName).ToList();
                       break;
                   case "orderAsc":
                       questionGroupLst = questionGroupLst.OrderBy(b => b.GroupOrder).ToList();
                       break;
                   case "orderDesc":
                       questionGroupLst = questionGroupLst.OrderByDescending(b => b.GroupOrder).ToList();
                       break;
                 
                   case "":
                       questionGroupLst = questionGroupLst.OrderBy(b => b.GroupName).ToList();
                       break;
               }
           }

           return questionGroupLst;
       }
    }
}
