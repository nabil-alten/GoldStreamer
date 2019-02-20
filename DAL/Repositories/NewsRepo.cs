using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class NewsRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public NewsRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }

        public List<News> GetBySubCategory(int? subCategoryId)
        {
            if (subCategoryId == null)
                return _context.Set<News>().OrderByDescending(n => n.Date).ToList();
            return _context.Set<News>().Where(n => n.CategoryId == subCategoryId).OrderByDescending(n => n.Date).ToList();
        }
        public List<News> GetBySubCategory(int? subCategoryId, DateTime newsDate)
        {
            if (subCategoryId == null)
                return _context.Set<News>().OrderByDescending(n => n.Date).Where(n => n.Date.Month == newsDate.Month && n.Date.Year == newsDate.Year && n.IsActive).ToList();
            return _context.Set<News>().Where(n => n.CategoryId == subCategoryId && n.Date.Month == newsDate.Month && n.Date.Year == newsDate.Year && n.IsActive).OrderByDescending(n => n.Date).ToList();
        }

        public List<News> GetTopViewsBySubCategory(int? subCategoryId)
        {
            if (subCategoryId == null)
                return _context.Set<News>().Where(n => n.IsActive).OrderByDescending(n => n.ViewCount).Take(5).ToList();
            return _context.Set<News>().Where(n => n.CategoryId == subCategoryId && n.IsActive).OrderByDescending(n => n.ViewCount).Take(5).ToList();
        }

        public News FindWithCategory(int id)
        {
            return _context.Set<News>().AsNoTracking().Include("newsCategory").FirstOrDefault(n => n.Id == id);
        }
        public List<News> SearchByAny(string title, int? category, int? mainCategory, DateTime? date)
        {
            List<News> news = _context.Set<News>().AsNoTracking().Include("NewsCategory").ToList();
            if (title != "")
                news = news.Where(n => n.Title.ToLower().Contains(title.Trim().ToLower())).ToList();
            if (category != 0 && category != null)
                news = news.Where(n => n.CategoryId == (int)category).ToList();
            if (mainCategory != 0 && mainCategory != null)
                news = news.Where(n => n.NewsCategory.MainCategoryId == (int)mainCategory).ToList();
            if (date != null)
                news = news.Where(n => n.Date.Date == Convert.ToDateTime(date).Date).ToList();
            return news;

            //if (category != 0 && category != null && mainCategory != 0 && mainCategory != null)
            //{
            //    return _context.Set<News>().Include("NewsCategory").Where(x => x.Title.ToLower().Contains(title.Trim().ToLower()) || (x.CategoryId == category && x.NewsCategory.MainCategoryId == mainCategory) || (date != null ? (x.Date.Day == date.Value.Day && x.Date.Month == date.Value.Month && x.Date.Year == date.Value.Year) : x.Date == date)).ToList();
            //}
            //else if (category == 0 && mainCategory != 0 && mainCategory != null)
            //{
            //    return _context.Set<News>().Include("NewsCategory").Where(x => x.Title.ToLower().Contains(title.Trim().ToLower()) || x.NewsCategory.MainCategoryId == mainCategory || (date != null ? (x.Date.Day == date.Value.Day && x.Date.Month == date.Value.Month && x.Date.Year == date.Value.Year) : x.Date == date)).ToList();
            //}
            //else if (mainCategory == 0 && category != 0 && category != null)
            //{
            //    return _context.Set<News>().Include("NewsCategory").Where(x => x.Title.ToLower().Contains(title.Trim().ToLower()) || x.CategoryId == category || (date != null ? (x.Date.Day == date.Value.Day && x.Date.Month == date.Value.Month && x.Date.Year == date.Value.Year) : x.Date == date)).ToList();
            //}
            //else if (mainCategory == 0 && category == 0)
            //{
            //    return _context.Set<News>().Include("NewsCategory").Where(x => x.Title.ToLower().Contains(title.Trim().ToLower()) || (date != null ? (x.Date.Day == date.Value.Day && x.Date.Month == date.Value.Month && x.Date.Year == date.Value.Year) : x.Date == date)).ToList();
            //}
            //else
            //{
            //    return _context.Set<News>().Include("NewsCategory").Where(x => x.Title.ToLower().Contains(title.Trim().ToLower()) || x.CategoryId == category || x.NewsCategory.MainCategoryId == mainCategory || (date != null ? (x.Date.Day == date.Value.Day && x.Date.Month == date.Value.Month && x.Date.Year == date.Value.Year) : x.Date == date)).ToList();
            //}
        }
        public void Delete(News n)
        {

            _context.Set<News>().Remove(n);
        }

        public List<News> GetOrderedNews()
        {

            return _context.Set<News>().Include("newsCategory").AsNoTracking().Where(n => n.IsLatest && n.IsActive).OrderBy(n => n.Order).ThenByDescending(n => n.Date).ToList();
        }

        public List<News> GetLatestNewsCount(int categoryId)
        {
            return _context.Set<News>().Include("newsCategory").AsNoTracking().Where(n => n.IsLatest && n.CategoryId == categoryId).OrderBy(n => n.Order).ToList();
        }

        public List<News> GetMonthNews(DateTime newsDate)
        {
            return
                _context.Set<News>()
                    .AsNoTracking()
                    .Include("newsCategory")
                    .Where(n => n.Date.Month == newsDate.Month && n.Date.Year == newsDate.Year)
                    .ToList();
        }

        public int GetNewsSearchCount(int? subCategoryId, DateTime? newsDate)
        {
            if (subCategoryId == null)
            {
                if (newsDate == null)
                    return _context.Set<News>().Where(n => n.IsActive).OrderByDescending(n => n.Date).ToList().Count;
                return _context.Set<News>().OrderByDescending(n => n.Date).Where(n => n.Date.Month == newsDate.Value.Month && n.Date.Year == newsDate.Value.Year && n.IsActive).ToList().Count;
            }
            if (newsDate == null)
                return _context.Set<News>().Where(n => n.CategoryId == subCategoryId && n.IsActive).OrderByDescending(n => n.Date).ToList().Count;
            return _context.Set<News>().Where(n => n.CategoryId == subCategoryId && n.Date.Month == newsDate.Value.Month && n.Date.Year == newsDate.Value.Year && n.IsActive).OrderByDescending(n => n.Date).ToList().Count;
        }

        public List<News> PagedNews(int? subCategoryId, DateTime? newsDate, int currentPage, int pageSize)
        {
            int skipedRec = currentPage * pageSize;
            //skipedRec = skipedRec == 0 ? -1 : skipedRec;
            if (subCategoryId == null)
            {
                if (newsDate == null)
                    return _context.Set<News>().Where(n => n.IsActive).Include("NewsCategory").OrderByDescending(n => n.Date).Skip(skipedRec).Take(pageSize).ToList();
                return _context.Set<News>().Where(n => n.IsActive).Include("NewsCategory").OrderByDescending(n => n.Date).Where(n => n.Date.Month == newsDate.Value.Month && n.Date.Year == newsDate.Value.Year).Skip(skipedRec).Take(pageSize).ToList();
            }
            if (newsDate == null)
                return _context.Set<News>().Include("NewsCategory").Where(n => n.CategoryId == subCategoryId && n.IsActive).OrderByDescending(n => n.Date).Skip(skipedRec).Take(pageSize).ToList();
            return _context.Set<News>().Include("NewsCategory").Where(n => n.CategoryId == subCategoryId && n.Date.Month == newsDate.Value.Month && n.Date.Year == newsDate.Value.Year && n.IsActive).OrderByDescending(n => n.Date).Skip(skipedRec).Take(pageSize).ToList();
        }

        public List<News> GetRelatedNews(int subCat, int newsId)
        {
            return _context.Set<News>().Where(n => n.CategoryId == subCat && n.IsActive && n.Id != newsId).OrderByDescending(n => n.Date).Take(5).ToList();
        }

        public List<NewsCategory> GetCategoriesWithNews()
        {
            return _context.Set<NewsCategory>().Include("News").Where(c => c.News.Where(n=>n.IsActive).Count()>0).ToList();
        }

    }
}
