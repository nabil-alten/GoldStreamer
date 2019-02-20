using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;

namespace DAL.Repositories
{
        public class QuestionRepo<T> : BaseRepo<T> where T : class
        {
            private readonly GoldStreamerContext _context;
            public QuestionRepo(GoldStreamerContext dbc) : base(dbc)
            {
                _context = dbc;
            }

            //public void DeleteAll()
            //{
            //    _context.Set<Question>().RemoveRange(GetAll());
            //}


            public List<Question> GetAll()
            {
              //  return _context.Set<QuestionGroup>().Select(q => q.Questions).Any().ToList();

                 return _context.Set<Question>().ToList();
            }

            public List<Question> FindByGroupId(int groupId)
            {
                return _context.Set<Question>().Where(g => g.QuestionGroupId == groupId).ToList();
            }

            //public void DeleteQuestion(Question QuestionObj)
            //{
            //    _context.Set<Question>().Remove(QuestionObj);
            //}

            //public void AddQuestion(Question QuestionObj)
            //{
            //    _context.Set<Question>().Add(QuestionObj);
            //}

            //public Question FindById(int questionId)
            //{
            //    return _context.Set<Question>().First(q => q.ID == questionId);
            //}

            public List<Question> Search(string searchText, int? isActive, bool? UserQuestion, bool? NoCategoryQuestions, bool? NoAnswersQuestions, bool? QuestionsNoAnswer)
            {
                var questionList = _context.Set<Question>().ToList();
                if (UserQuestion != null)
                {
                    if (bool.Parse( UserQuestion.ToString()))
                    {
                        questionList = questionList.Where(q => q.IsUserQuestion == true)
                            .ToList();
                        if (!bool.Parse(NoCategoryQuestions.ToString()))
                        {
                            questionList = questionList.Where(q => q.QuestionGroupId == null)
                          .ToList();
                        }
                        else
                        {
                            questionList = questionList.Where(q => q.QuestionGroupId != null)
                         .ToList();
                        }
                        if (!bool.Parse(NoAnswersQuestions.ToString()))
                        {
                            questionList = questionList.Where(q => q.Answer == null)
                        .ToList();
                        }
                        else
                        {
                            questionList = questionList.Where(q => q.Answer != null)
                       .ToList();
                        }
                        if (!bool.Parse(QuestionsNoAnswer.ToString()))
                        {
                            questionList = questionList.Where(q => q.IsReplied == false)
                       .ToList();
                        }
                        else
                        {
                            questionList = questionList.Where(q => q.IsReplied == true)
                      .ToList();
                        }
                    }
                }
                if (isActive != null)
                {
                    if (int.Parse(isActive.ToString()) == 1)
                    {
                        questionList = questionList.Where(q => q.IsActive == true)
                                           .ToList();
                    }
                    else if (int.Parse(isActive.ToString()) == 2)
                    {
                        questionList = questionList.Where(q => q.IsActive == false)
                                           .ToList();
                    }
                }
                foreach (var questionItem in questionList)
                {
                    if (questionItem.QuestionGroupId != null)
                    {
                        questionItem.QuestionGroup =
                            _context.Set<QuestionGroup>().First(g => g.ID == questionItem.QuestionGroupId).GroupName;
                    }
                    else
                    {
                        questionItem.QuestionGroup = "";
                    }
                }
                if(searchText!=null&&searchText!="")
                {
                    searchText = searchText.TrimEnd(' ');
                    searchText = searchText.TrimStart(' ');
                    questionList=   questionList.Where(q => q.QuestionText.ToLower().Contains(searchText.ToLower()) || q.QuestionGroup.ToLower().Contains(searchText.ToLower()))
                        .ToList();
                }

               
                return questionList.ToList();
            }
            public void ChangeActiveAssignment(Question question)
            {
                List<Question> lstQuestions = new List<Question>();
                if (question.IsActive)
                {
                    lstQuestions = _context.Set<Question>().Where(c => c.IsActive && c.ID != question.ID).ToList();
                    if (lstQuestions.Count() != 0)
                    {
                        foreach (Question b in lstQuestions)
                        {
                            b.IsActive = false;
                            _context.Entry(b).State = EntityState.Modified;
                        }
                    }
                }



            }
            public List<Question> GetActiveQuestions(Question question)
            {
                List<Question> lstQuestions = new List<Question>();
                if (question.IsActive)
                {
                    lstQuestions = _context.Set<Question>().Where(c => c.IsActive && c.ID != question.ID).ToList();
                    
                }
                return lstQuestions;


            }
            public bool CheckIfQuestionExist(string questionText , int questionId)
            {
                Question x = null;
                if (questionId == 0)
                {

                    x = _context.Set<Question>().FirstOrDefault(g => g.QuestionText == questionText.Trim() && g.IsUserQuestion==false);
                }

                else
                {
                    x = _context.Set<Question>().FirstOrDefault(g => g.QuestionText == questionText.Trim() && g.ID != questionId && g.IsUserQuestion==false);
 
                }
                if (x != null) return true;
                return false;
            }
            public int CheckUserQuestionExistMax(string questionText)
            {
                var x =  _context.Set<Question>().Where(g => g.QuestionText == questionText.Trim()).ToList();
               if(x.Count()!=0)
               {
                   return x.Max(q => q.ID);
               }
               else
               {
                   return 0;
               }
               
            }
        }

}
