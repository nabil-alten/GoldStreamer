using BLL.DomainClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DAL.Repositories
{
    public class NewsMainCategoryRepo<T> : BaseRepo<T> where T : class
    {
        GoldStreamerContext _context = null;
        public NewsMainCategoryRepo(GoldStreamerContext dbContext)
        {
            _context = dbContext;
        }

        public List<NewsMainCategory> GetNewsMainCatetoryList(string searchText)
        {
            return _context.Set<NewsMainCategory>()
                .Where(b => b.MainCategoryName.Contains(searchText) || (b.MainCategoryOrder.ToString().Contains(searchText)) || string.IsNullOrEmpty(searchText)).ToList();
        }
        public bool CanAddMainCategory(string mainCategoriesCount)
        {
            if (_context.NewsMainCategory.ToList().Count < int.Parse(mainCategoriesCount))
                return true;
            else
                return false;
        }

        public bool IsValidOrder (string mainCategoriesCount, int UpdatedOrder)
        {
            if (UpdatedOrder <= int.Parse(mainCategoriesCount))
                return true;
            else
                return false;
        }
        public void AddNewsMainCategory(NewsMainCategory newNewsMainCategory)
        {
            _context.NewsMainCategory.Add(newNewsMainCategory);
        }
        public void UpdateNewsMainCategory(NewsMainCategory oldNewsMainCategory)
        {
            _context.Entry(oldNewsMainCategory).State = EntityState.Modified;
        }
        public void DeleteByID(int id)
        {
            NewsMainCategory deleted = _context.NewsMainCategory.Find(id);
            _context.NewsMainCategory.Remove(deleted);
        }
        public void DeleteAllMainCategories()
        {
            _context.Set<News>().RemoveRange(_context.News.ToList());
            _context.Set<NewsCategory>().RemoveRange(_context.NewsCategory.ToList());
            _context.Set<NewsMainCategory>().RemoveRange(_context.NewsMainCategory.ToList());
        }
        public int CheckNameExists(string MainCategoryName, int? id)
        {
            NewsMainCategory group =
               _context.Set<NewsMainCategory>()
                       .FirstOrDefault(
                           b => b.MainCategoryName == MainCategoryName.Trim() && (id == null || b.ID != id));
            return group == null ? 0 : 1;
        }
        public int CheckOrderExists(int order, int? id)
        {
            NewsMainCategory group =
               _context.Set<NewsMainCategory>()
                       .FirstOrDefault(
                           b => b.MainCategoryOrder == order && (id == null || b.ID != id));
            return group == null ? 0 : 1;
        }

        public bool HasCategories(int ID)
        {
            List<NewsCategory> categories = _context.Set<NewsCategory>().Where(b => b.MainCategoryId == ID).ToList();
            if (categories.Count == 0)
                return false;
            else
                return true;
        }

        public NewsMainCategory FindNewsMainCategoryByName(string name)
        {
            NewsMainCategory newsMainCategory =
                          _context.Set<NewsMainCategory>()
                                  .FirstOrDefault(
                                      b => b.MainCategoryName == name.Trim());
            return newsMainCategory;
        }
    }
}
