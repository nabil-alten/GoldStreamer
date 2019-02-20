using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using localization.Helpers;

namespace GoldStreamer.Controllers
{
    [System.Web.Mvc.Authorize(Roles = "Admin")]
    [Internationalization]
    public class NewsCategoryController : Controller
    {
        UnitOfWork _uow = new UnitOfWork();
        public ActionResult NewsCategory(int? MainCategoryID)
        {
            var allMainCategories = _uow.NewsMainCategoryRepo.GetNewsMainCatetoryList("");
            //ViewData["MainCategories"] = new SelectList(allMainCategories, "ID", "MainCategoryName", CategoryID);
            ViewData["MainCategory"] = _uow.NewsMainCategoryRepo.Find(MainCategoryID).MainCategoryName;
            return View();
        }
        public ActionResult _Save()
        {
            NewsCategory newObj = new NewsCategory();
            return PartialView("_Save", newObj);
        }
        [AllowAnonymous]
        public ActionResult GetCategory(int mainCategory)
        {
            List<NewsCategory> newscategory = _uow.NewsCategoryRepo.GetByMainCategory(mainCategory);
            //newscategory.Insert(0, new NewsCategory { CategoryName = "اختر", Id = 0 });

            return Json(new { newscategory }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult _List(string searchText, int MainCategoryID)
        {
            ViewBag.GroupsSearchText = searchText;
            List<NewsCategory> NewsCategoryLst = _uow.NewsCategoryRepo.GetNewsCatetoryList(searchText, MainCategoryID);
            return PartialView("_List", NewsCategoryLst);
        }

        [HttpPost]
        public ActionResult _Save(NewsCategory _Category, int MainCategoryID)
        {
            try
            {
                if (_Category.Id == 0)  //Add
                {
                    if (_uow.NewsCategoryRepo.CanAddCategory(System.Configuration.ConfigurationManager.AppSettings["CategoriesNumber"], MainCategoryID))
                    {
                        if (_uow.NewsCategoryRepo.CheckNameExists(_Category.CategoryName, null, MainCategoryID) == 1)
                            return new HttpStatusCodeResult(1);
                        if (_uow.NewsCategoryRepo.CheckOrderExists(_Category.CategoryOrder, null, MainCategoryID) == 1)
                            return new HttpStatusCodeResult(5);

                        if (_uow.NewsCategoryRepo.IsValidOrder(System.Configuration.ConfigurationManager.AppSettings["CategoriesNumber"], _Category.CategoryOrder))
                        {
                            //_Category.MainCategoryId = CategoryID;
                            _uow.NewsCategoryRepo.AddNewsCategory(_Category);
                            _uow.SaveChanges();
                        }
                        else
                            return new HttpStatusCodeResult(3);
                    }
                    else
                        return new HttpStatusCodeResult(2);

                }
                else  //Update
                {
                    if (_uow.NewsCategoryRepo.CheckNameExists(_Category.CategoryName, _Category.Id, MainCategoryID) == 1)
                        return new HttpStatusCodeResult(1);
                    if (_uow.NewsCategoryRepo.CheckOrderExists(_Category.CategoryOrder, _Category.Id, MainCategoryID) == 1)
                        return new HttpStatusCodeResult(5);

                    if (_uow.NewsCategoryRepo.IsValidOrder(System.Configuration.ConfigurationManager.AppSettings["CategoriesNumber"], _Category.CategoryOrder))
                    {
                        //_Category.MainCategoryId = MainCategoryID;
                        _uow.NewsCategoryRepo.UpdateNewsCategory(_Category);
                        _uow.SaveChanges();
                    }
                    else
                        return new HttpStatusCodeResult(3);
                }

                return PartialView("_List", _uow.NewsCategoryRepo.GetAll());
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetCategoryInfo(int id)
        {
            NewsCategory category = _uow.NewsCategoryRepo.FindNewsCategoryByID(id);
            string result = "Name:" + category.CategoryName + ",Order:" + category.CategoryOrder + ",MainCategory:" + category.MainCategory.ID;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
        NewsCategory nc=_uow.NewsCategoryRepo.Find(id);
            _uow.NewsCategoryRepo.Delete(nc);
            _uow.SaveChanges();

            return Json(_uow.NewsCategoryRepo.GetAll(), JsonRequestBehavior.AllowGet);
        }
	}
}