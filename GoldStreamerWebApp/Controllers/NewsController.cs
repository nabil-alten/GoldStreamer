using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using GoldStreamerWebApp.Helpers;
using localization.Helpers;
using PagedList;
using System.Configuration;
using GoldStreamer.Filters;

namespace GoldStreamer.Controllers
{
    [AdvancedAuthorize, Authorize(Roles = "Admin")]
    [Internationalization]
    public class NewsController : Controller
    {
        UnitOfWork uow = new UnitOfWork();
        [AllowAnonymous]
        public ActionResult Index(int? subCategoryId, DateTime? newsDate, int? indx, int? pageNo)
        {
            //int totalCount = uow.NewsRepo.GetNewsSearchCount(subCategoryId, newsDate);

            //ViewBag.totalCount = totalCount;
            //ViewBag.pageNo = pageNo ?? 0;
            //ViewBag.subCategoryId = subCategoryId;
            ViewBag.newsDate = newsDate;
            //ViewBag.selected = indx;


            //// List<News> news = newsDate == null ? uow.NewsRepo.GetBySubCategory(subCategoryId) : uow.NewsRepo.GetBySubCategory(subCategoryId, newsDate.Value);
            //List<News> news = uow.NewsRepo.PagedNews(subCategoryId, newsDate, pageNo ?? 0,2);
            ////ViewBag.selected = 0;
            //ViewBag.subCategoryName = subCategoryId != null ? uow.NewsCategoryRepo.FindNewsCategoryByID(int.Parse(subCategoryId.ToString())).CategoryName : "";


            //return View(news);
            ViewBag.selected = indx ?? 0;
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult _NewsList(int? subCategoryId, string newsDate, int? indx, int? pageNo)
        {
            subCategoryId = subCategoryId == -1 ? null : subCategoryId;
            if (newsDate == null) newsDate = DateTime.Now.ToShortDateString();
            int totalCount = uow.NewsRepo.GetNewsSearchCount(subCategoryId, Convert.ToDateTime(newsDate));

            ViewBag.totalCount = totalCount;
            ViewBag.pageNo = pageNo ?? 0;
            ViewBag.subCategoryId = subCategoryId;
            ViewBag.newsDate = newsDate;
            ViewBag.selected = indx;
            //NewsPaging
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["NewsPaging"]);

            // List<News> news = newsDate == null ? uow.NewsRepo.GetBySubCategory(subCategoryId) : uow.NewsRepo.GetBySubCategory(subCategoryId, newsDate.Value);
            List<News> news = uow.NewsRepo.PagedNews(subCategoryId, Convert.ToDateTime(newsDate), pageNo ?? 0, pageSize);
            //ViewBag.selected = 0;
            ViewBag.subCategoryName = subCategoryId != null ? uow.NewsCategoryRepo.FindNewsCategoryByID(int.Parse(subCategoryId.ToString())).CategoryName : "";

            return PartialView(news);
            //return View(news);
        }

        public ActionResult Add()
        {
            var newsMainCategories = uow.NewsMainCategoryRepo.GetAll();
            //govs.Insert(0,new Governorate(){Code="",Name="اختر",ID = 0});
            ViewBag.newsMainCategories = new SelectList(newsMainCategories, "ID", "MainCategoryName");

            return View("Add");
        }

        [HttpPost]
        public ActionResult Add(News news)
        {
            int maxLogoLength = 4096; // 5 MB

            var newsMainCategories = uow.NewsMainCategoryRepo.GetAll();
            ViewBag.newsMainCategories = new SelectList(newsMainCategories, "ID", "MainCategoryName");

            int orderSettings = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NewsOrder"]);
            int latestSettings = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxNewsHome"]);

            if (news.Order > orderSettings)
            {
                TempData[AlertStyles.Error] = Resources.Messages.NewsOrder;
                return View("Add");
            }

            if (news.IsLatest)
            {
                int currentLatest = uow.NewsRepo.GetLatestNewsCount(news.CategoryId).Count;
                if (news.IsLatest) currentLatest++;

                if (currentLatest > latestSettings)
                {
                    TempData[AlertStyles.Error] = Resources.Messages.LatestNewsLimit;
                    return View("Add");
                }
            }

            HttpPostedFileBase file = Request.Files["File1"];
            //if (file.FileName == "")
            //{
            //    TempData[AlertStyles.Error] = Resources.Messages.NewsPicRequired;
            //    //ViewBag.ErrorMessage = Resources.Messages.CompanyLogoLength;
            //    return View("Add");
            //}
            if (file != null && file.ContentLength / 1024 > maxLogoLength)
            {
                TempData[AlertStyles.Error] = Resources.Messages.CompanyLogoLength;
                //ViewBag.ErrorMessage = Resources.Messages.CompanyLogoLength;
                return View("Add");
            }

            if (Request.Files != null && Request.Files.Count > 0)
            {

                if (file != null && file.ContentLength > 0)
                {
                    // Code to process image, resize, etc goes here
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    if (extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".tiff" &&
                        extension != ".bmp" && extension != ".gif")
                    {
                        TempData[AlertStyles.Error] = Resources.Messages.CompanyLogoType;
                        //ViewBag.ErrorMessage = Resources.Messages.CompanyLogoType;
                        return View("Add");
                    }

                }
            }
            news.Date = news.Date;//.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
            uow.NewsRepo.Add(news);
            uow.SaveChanges();

            // save pic
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/Uploads/News/"), news.Id + ".png");
            file.SaveAs(path);


            var bytes = new byte[0];
            ViewBag.ImageData = "image/png";

            if (Request.Files.Count == 1)
            {

                string var_path = Server.MapPath("~/Uploads/News/" + news.Id + ".png"); //fileName);
                byte[] imageByteData = System.IO.File.ReadAllBytes(var_path);
                string imageBase64Data = Convert.ToBase64String(imageByteData);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                ViewBag.ImageData = var_path;
                news.Photo = file.FileName != "" ? news.Id + ".png" : null;
                uow.SaveChanges();
            }

            TempData[AlertStyles.Success] = Resources.Messages.Saved;
            //TempData["shortMessage"] = Resources.Messages.Saved;
            return RedirectToAction("Add"); //View("Add",new News());
        }
        [AllowAnonymous]
        public ActionResult Details(int newsId)
        {
            News news = uow.NewsRepo.Find(newsId);
            news.ViewCount += 1;
            uow.SaveChanges();

            return View("NewsDetails", news);
        }

        public ActionResult List()
        {
            List<News> news = uow.NewsRepo.GetBySubCategory(null);
            return View("ListAdmin", news);
        }

        public ActionResult Edit(int? newsId)
        {
            if (newsId == null)
                return RedirectToAction("ListIndex");

            News news = uow.NewsRepo.FindWithCategory(newsId.Value);
            var newsMainCategories = uow.NewsMainCategoryRepo.GetAll();
            ViewBag.newsMainCategories = new SelectList(newsMainCategories, "ID", "MainCategoryName", news.NewsCategory.MainCategoryId);

            List<NewsCategory> newsCategories = uow.NewsCategoryRepo.GetByMainCategory(news.NewsCategory.MainCategoryId);
            ViewBag.CategoryId = new SelectList(newsCategories, "Id", "CategoryName", news.CategoryId);
            //ViewBag.ErrorMessage = TempData["shortMessage"] == null ? "" : TempData["shortMessage"].ToString();

            return View(news);
        }

        [HttpPost]
        public ActionResult Edit(News news)
        {
            try
            {

                int maxLogoLength = 4096; // 5 MB

                //var newsMainCategories = uow.NewsMainCategoryRepo.GetAll();
                //ViewBag.newsMainCategories = new SelectList(newsMainCategories, "ID", "MainCategoryName");
                int orderSettings = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["NewsOrder"]);
                int latestSettings = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxNewsHome"]);

                var newsMainCategories = uow.NewsMainCategoryRepo.GetAll();
                ViewBag.newsMainCategories = new SelectList(newsMainCategories, "ID", "MainCategoryName");

                if (news.Order > orderSettings)
                {
                    TempData[AlertStyles.Error] = Resources.Messages.NewsOrder;
                    return RedirectToAction("Edit", new { newsId = news.Id });
                    return View("Edit", news);
                }

                News news1 = uow.NewsRepo.FindWithCategory(news.Id);
                if (news.IsLatest)
                {
                    int currentLatest = uow.NewsRepo.GetLatestNewsCount(news.CategoryId).Count;

                    if (news.IsLatest && !news1.IsLatest) currentLatest++;

                    if (currentLatest > latestSettings)
                    {
                        TempData[AlertStyles.Error] = Resources.Messages.LatestNewsLimit;
                        return RedirectToAction("Edit", new { newsId = news.Id });
                    }
                }

                HttpPostedFileBase file = Request.Files["File1"];
                //if (file == null && news.Photo == null)
                //{
                //    TempData[AlertStyles.Error] = Resources.Messages.NewsPicRequired;
                //    //ViewBag.ErrorMessage = Resources.Messages.CompanyLogoLength;
                //    return View("Add");
                //}

                if (file != null && file.ContentLength / 1024 > maxLogoLength)
                {
                    TempData[AlertStyles.Error] = Resources.Messages.CompanyLogoLength;
                    return RedirectToAction("Edit", new { newsId = news.Id });
                    return View("Edit", news);
                }

                if (Request.Files != null && Request.Files.Count > 0)
                {

                    if (file.FileName != "" && file.ContentLength > 0)
                    {
                        // Code to process image, resize, etc goes here
                        string extension = Path.GetExtension(file.FileName).ToLower();
                        if (extension != ".png" && extension != ".jpg" && extension != ".jpeg" && extension != ".tiff" &&
                            extension != ".bmp" && extension != ".gif")
                        {
                            TempData[AlertStyles.Error] = Resources.Messages.CompanyLogoType;
                            return RedirectToAction("Edit", new { newsId = news.Id });
                            return View("Edit", news);
                        }

                    }
                }
                news.Date = news.Date.Minute == 0
                    ? news.Date.AddHours(DateTime.Now.Hour)
                        .AddMinutes(DateTime.Now.Minute)
                        .AddSeconds(DateTime.Now.Second)
                    : news.Date;
                news.Photo = news1.Photo;
                uow.NewsRepo.Update(news);

                uow.SaveChanges();

                #region save pic

                if (file.FileName != "")
                {
                    System.IO.File.Delete(Server.MapPath("~/Uploads/News/" + news.Id + ".png"));
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Uploads/News/"), news.Id + ".png");
                    file.SaveAs(path);


                    var bytes = new byte[0];
                    ViewBag.ImageData = "image/png";

                    if (Request.Files.Count == 1)
                    {

                        string var_path = Server.MapPath("~/Uploads/News/" + news.Id + ".png"); //fileName);
                        byte[] imageByteData = System.IO.File.ReadAllBytes(var_path);
                        string imageBase64Data = Convert.ToBase64String(imageByteData);
                        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                        ViewBag.ImageData = var_path;
                        news.Photo = file.FileName != "" ? news.Id + ".png" : null;
                        uow.SaveChanges();
                    }
                }

                #endregion

                //news.NewsCategory = uow.NewsCategoryRepo.Find(news.CategoryId);
                TempData[AlertStyles.Success] = Resources.Messages.Updated;
                //TempData["shortMessage"] = Resources.Messages.Saved;
            }
            catch (Exception ex)
            {
                TempData[AlertStyles.Error] = Resources.Messages.UpdateFailed;
                //ViewBag.ErrorMessage = Resources.Messages.SaveFailed;
                return RedirectToAction("Edit", new { newsId = news.Id });
            }
            return RedirectToAction("Edit", new { newsId = news.Id }); //View("Edit",news.Id);
        }
        [AllowAnonymous]
        public PartialViewResult _MostViewed(int? subCategoryId)
        {
            subCategoryId = subCategoryId == -1 ? null : subCategoryId;
            //subCat=ViewBag.subCategoryId;
            List<News> mostViewed = uow.NewsRepo.GetTopViewsBySubCategory(subCategoryId);
            return PartialView("_MostViewed", mostViewed);
        }

        [AllowAnonymous]
        public PartialViewResult _News()
        {
            UnitOfWork uow = new UnitOfWork();
            return PartialView("../Home/_News", uow.NewsRepo.GetOrderedNews());

        }

        public ActionResult ListIndex(string title, int? mainCategory, int? subcategory, DateTime? date, int? pageSize)
        {
            ViewBag.PageSize = pageSize;
            ViewBag.Newstitle = title;
            ViewBag.MainCategory = mainCategory;
            ViewBag.SubCategory = subcategory;
            ViewBag.date = date;
            int x = 0;
            int y = Convert.ToInt32(pageSize);
            PageMethod(out x, out y);
            List<News> lst = new List<News>();
            if (!string.IsNullOrEmpty(title) || (mainCategory != null && mainCategory != 0) || (subcategory != null && subcategory != 0) || date != null)
            {
                lst = uow.NewsRepo.SearchByAny(title, subcategory, mainCategory, date).OrderByDescending(c => c.Date).ToList();
                ViewBag.count = lst.Count();
            }
            else
            {
                lst = uow.NewsRepo.GetAll().OrderByDescending(c => c.Date).ToList();
            }
            return View(lst.ToPagedList(x, y));
        }
        public PartialViewResult _List(string title, int? mainCategory, int? subcategory, DateTime? date, int? pageSize, int? pageNumber)
        {

            ViewBag.PageSize = pageSize;
            ViewBag.Newstitle = title;
            ViewBag.MainCategory = mainCategory;
            ViewBag.SubCategory = subcategory;
            ViewBag.date = date;
            ViewBag.PageNumber = pageNumber;

            //List<ApplicationUser> AlluSERS = uow.UsersRepo.BlockUnBolckList().ToList();
            //List<ApplicationUser> USERS = SortBy("FirstName_asc", AlluSERS);
            int x = 0;
            int y = Convert.ToInt32(pageSize);
            if (pageNumber == null)
            {
                x = 0;
            }
            else
            {
                x = (int)pageNumber;
            }
            if (pageNumber == 0) pageNumber = 1;
            PageMethod(out x, out y);
            List<News> lst = new List<News>();
            if (!string.IsNullOrEmpty(title) || (mainCategory != null && mainCategory != 0) || (subcategory != null && subcategory != 0) || date != null)
            {

                lst = uow.NewsRepo.SearchByAny(title, subcategory, mainCategory, date).OrderByDescending(c => c.Date).ToList();
                ViewBag.count = lst.Count();
                return PartialView("_List", lst.ToPagedList(x, y));
            }
            else
            {
                lst = uow.NewsRepo.GetAll().OrderByDescending(c => c.Date).ToList();
            }
            ViewBag.count = lst.Count();
            return PartialView("_List", lst.ToPagedList(x, y));
        }
        [HttpPost]
        public ActionResult UnActivateNews(string statusId, int pageNumber)
        {

            ViewBag.PageNumber = pageNumber;

            var cut_id = statusId;
            var status = cut_id.Substring(0, 1);
            var id = cut_id.Substring(2, cut_id.Length - 2);
            News NewsObj = uow.NewsRepo.Find(int.Parse(id.ToString()));
            if (status == "1")
            {
                NewsObj.IsActive = true;
            }
            else
            {
                NewsObj.IsActive = false;
            }
            uow.NewsRepo.Update(NewsObj);
            uow.SaveChanges();
            List<News> lst = uow.NewsRepo.GetAll().OrderByDescending(x => x.Date).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            //   int pageNumber = 1;
            ViewBag.pageSize = pageSize;
            return PartialView("_List", lst.ToPagedList(pageNumber, pageSize));
        }
        public void PageMethod(out int pageNum, out int pageSiz)
        {
            string page = Request.QueryString["PageNumber"];
            string pageSize = Request.QueryString["pageSize"];
            if (page == null)
            {
                page = "1";
            }
            else
            {
                page = Request.QueryString["PageNumber"].ToString();
            }
            if (pageSize == null || pageSize == ConfigurationManager.AppSettings["DefaultPageSize"].ToString())
            {
                pageSiz = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"].ToString());
            }
            else
            {
                pageSiz = int.Parse(pageSize.ToString());
            }
            pageNum = 0;
            if (!string.IsNullOrEmpty(page))
            {
                pageNum = ((int?)int.Parse(page) ?? 1);
            }

        }
        [HttpPost]
        public ActionResult Delete(int id, int pageNumber)
        {
            ViewBag.PageNumber = pageNumber;
            News news = uow.NewsRepo.Find(id);
            uow.NewsRepo.Delete(news);
            uow.SaveChanges();
            List<News> lst = uow.NewsRepo.GetAll().OrderByDescending(x => x.Date).ToList();
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            ViewBag.pageSize = pageSize;
            if (lst.ToPagedList(pageNumber, pageSize).Count() == 0)
            {
                pageNumber = pageNumber - 1;
            }
            return PartialView("_List", lst.ToPagedList(pageNumber, pageSize));

        }

        [AllowAnonymous]
        public ActionResult FilterNews(DateTime newsDate, int indx, int? pageSize)
        {
            List<News> news = uow.NewsRepo.GetMonthNews(newsDate);
            ViewBag.selected = indx;
            return View("Index", news);
        }

        //public ActionResult Next(DateTime? newsDate, int catId)
        //{

        //}

        [AllowAnonymous]
        public PartialViewResult _RelatedNews(int categoryId, int newsId)
        {
            List<News> relatedNews = uow.NewsRepo.GetRelatedNews(categoryId, newsId);
            return PartialView("_RelatedNews", relatedNews);
        }

        [AllowAnonymous]
        public PartialViewResult _NewsCategories()
        {
            List<NewsCategory> newsCat = uow.NewsRepo.GetCategoriesWithNews();
            return PartialView("_NewsCategories", newsCat);
        }

    }
}