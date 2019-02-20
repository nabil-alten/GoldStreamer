using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class QuestionGroupRepo<T> : BaseRepo<T> where T : class
    {
        GoldStreamerContext _context = null;
        public QuestionGroupRepo(GoldStreamerContext dbContext)
        {
            _context = dbContext;
        }

        public List<QuestionGroup> GetQuestionGroupList(string searchText)
        {
            return _context.Set<QuestionGroup>()
                .Where(b => b.GroupName.Contains(searchText) || (b.GroupOrder.ToString().Contains(searchText)) || string.IsNullOrEmpty(searchText)).ToList();
        }
        public void AddNewQuestionGroup(QuestionGroup newQusetionGroup)
        {
            _context.QuestionGroup.Add(newQusetionGroup);
        }
        public void UpdateQuestionGroup(QuestionGroup oldQuestionGroup)
        {
            _context.Entry(oldQuestionGroup).State = EntityState.Modified;
        }
        public void DeleteByID(int id)
        {
            QuestionGroup deleted = _context.QuestionGroup.Find(id);
            _context.QuestionGroup.Remove(deleted);
        }
        public int CheckNameExists(string GroupName, int? id)
        {
            QuestionGroup group =
               _context.Set<QuestionGroup>()
                       .FirstOrDefault(
                           b => b.GroupName == GroupName.Trim() && (id == null || b.ID != id));
            return group == null ? 0 : 1;
        }
        public int CheckOrderExists(int order, int? id)
        {
            QuestionGroup group =
               _context.Set<QuestionGroup>()
                       .FirstOrDefault(
                           b => b.GroupOrder == order && (id == null || b.ID != id));
            return group == null ? 0 : 1;
        }
        public bool hasAssignedQuestions(int id)
        {
            List<Question> assignedQuestions = _context.Set<Question>().Where(b => b.QuestionGroupId == id).ToList();
            if (assignedQuestions.Count == 0)
                return false;
            else
                return true;
        }

        public List<QuestionGroup> GetAllGroupsWithActiveQuestions()
        {
            return _context.Set<QuestionGroup>().Include(q => q.Questions).Where(x=>x.Questions.Any()).ToList();
        }

       
        public void DeleteAllQuestionGroups()
        {
            _context.Set<Question>().RemoveRange(_context.Set<Question>().ToList());
            _context.Set<QuestionGroup>().RemoveRange(_context.Set<QuestionGroup>().ToList());
        }
    }
}
