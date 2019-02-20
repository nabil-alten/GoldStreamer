using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class NewsCategoryRepo<T> : BaseRepo<T> where T : class
    {
         private readonly GoldStreamerContext _context;
         public NewsCategoryRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }

        public List<NewsCategory> GetByMainCategory(int mainCategory)
        {
            return _context.Set<NewsCategory>().Where(c => c.MainCategoryId == mainCategory).ToList();
        }

        public List<NewsCategory> GetNewsCatetoryList(string searchText, int? mainCategoryID)
        {
            return _context.Set<NewsCategory>().Include("MainCategory")
                .Where(b => (b.CategoryName.Contains(searchText) || (b.CategoryOrder.ToString().Contains(searchText)) || string.IsNullOrEmpty(searchText)) && (b.MainCategoryId == mainCategoryID || mainCategoryID == null)).ToList();
        }

        public NewsCategory FindNewsCategoryByID(int id)
        {
            return _context.Set<NewsCategory>().Include("MainCategory")
                .Where(b => b.Id == id).Single();
        }
        public void AddNewsCategory(NewsCategory newCategory)
        {
            _context.NewsCategory.Add(newCategory);
        }
        public void UpdateNewsCategory(NewsCategory oldNewsCategory)
        {
            _context.Entry(oldNewsCategory).State = EntityState.Modified;
        }
        public int CheckNameExists(string categoryName, int? id, int mainCategoryID)
        {
            NewsCategory group =
               _context.Set<NewsCategory>()
                       .FirstOrDefault(
                           b => b.CategoryName == categoryName.Trim() && b.MainCategoryId == mainCategoryID && (id == null || b.Id != id));
            return group == null ? 0 : 1;
        }
        public int CheckOrderExists(int order, int? id, int mainCategoryID)
        {
            NewsCategory group =
               _context.Set<NewsCategory>()
                       .FirstOrDefault(
                           b => b.CategoryOrder == order && b.MainCategoryId == mainCategoryID && (id == null || b.Id != id));
            return group == null ? 0 : 1;
        }
        public void DeleteByID(int id)
        {
            NewsCategory deleted = _context.NewsCategory.Find(id);
            _context.NewsCategory.Remove(deleted);
        }
        public void Delete(NewsCategory nc)
        {

            _context.Set<NewsCategory>().Remove(nc);
        }
        public bool CanAddCategory(string categoriesCount, int mainCategoryID)
        {
            List<NewsCategory> categoriesList = _context.Set<NewsCategory>().Where(b => b.MainCategoryId == mainCategoryID).ToList();
            if (categoriesList.ToList().Count < int.Parse(categoriesCount))
                return true;
            else
                return false;
        }

        public bool IsValidOrder(string validCategoriesCount, int order)
        {
            if (order <= int.Parse(validCategoriesCount))
                return true;
            else
                return false;
        }
    }
}
