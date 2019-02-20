using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using System.Collections.Generic;


namespace DAL.Repositories
{
  
        public class FeedbackRepo<T> : BaseRepo<T> where T : class
    {
         private readonly GoldStreamerContext _context;
         public FeedbackRepo(GoldStreamerContext dbc)
             : base(dbc)
        {
            _context = dbc;
        }


   
         public List<Feedback> GetAll()
         {
             return _context.Set<Feedback>().ToList() ;
         }

         public void AddFeedback(Feedback feedbackObj)
         {
             _context.Set<Feedback>().Add(feedbackObj);
         }

         public void DeleteAll()
         {
             _context.Set<Feedback>().RemoveRange(GetAll());
         }

    }
}
