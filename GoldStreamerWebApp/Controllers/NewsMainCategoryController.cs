using BLL.DomainClasses;
using DAL.UnitOfWork;
using localization.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoldStreamer.Controllers
{

    [System.Web.Mvc.Authorize(Roles = "Admin")]
    [Internationalization]
    public class NewsMainCategoryController : Controller
    {
        UnitOfWork _uow = null;

        public NewsMainCategoryController()
        {
            _uow = new UnitOfWork();
        }
        public ActionResult NewsMainCategory()
        {
            return View();
        }

        public PartialViewResult _List(string searchText)
        {
            ViewBag.GroupsSearchText = searchText;
            List<NewsMainCategory> NewsMainCategoryLst = _uow.NewsMainCategoryRepo.GetNewsMainCatetoryList(searchText);
            return PartialView("_List", NewsMainCategoryLst);
        }
        public ActionResult _Save()
        {
            NewsMainCategory newObj = new NewsMainCategory();
            return PartialView("_Save", newObj);
        }
        [HttpPost]
        public ActionResult _Save(NewsMainCategory _mainCategory)
        {
            try
            {
                if (_mainCategory.ID == 0)
                {
                    if (_uow.NewsMainCategoryRepo.CanAddMainCategory(System.Configuration.ConfigurationManager.AppSettings["MainCategoriesNumber"]))
                    {
                        if (_uow.NewsMainCategoryRepo.CheckNameExists(_mainCategory.MainCategoryName, null) == 1)
                            return new HttpStatusCodeResult(1);
                        if (_uow.NewsMainCategoryRepo.CheckOrderExists(_mainCategory.MainCategoryOrder, null) == 1)
                            return new HttpStatusCodeResult(4);


                        if (_uow.NewsMainCategoryRepo.IsValidOrder(System.Configuration.ConfigurationManager.AppSettings["MainCategoriesNumber"], _mainCategory.MainCategoryOrder))
                        {
                            _uow.NewsMainCategoryRepo.AddNewsMainCategory(_mainCategory);
                            _uow.SaveChanges();
                        }
                        else
                            return new HttpStatusCodeResult(3);
                    }
                    else
                    {
                        return new HttpStatusCodeResult(2);
                    }
                }
                else
                {
                    if (_uow.NewsMainCategoryRepo.CheckNameExists(_mainCategory.MainCategoryName, _mainCategory.ID) == 1)
                        return new HttpStatusCodeResult(1);
                    if (_uow.NewsMainCategoryRepo.CheckOrderExists(_mainCategory.MainCategoryOrder, _mainCategory.ID) == 1)
                        return new HttpStatusCodeResult(4);

                    if (_uow.NewsMainCategoryRepo.IsValidOrder(System.Configuration.ConfigurationManager.AppSettings["MainCategoriesNumber"], _mainCategory.MainCategoryOrder))
                    {
                        _uow.NewsMainCategoryRepo.UpdateNewsMainCategory(_mainCategory);
                        _uow.SaveChanges();
                    }
                    else
                        return new HttpStatusCodeResult(3);
                }


                return PartialView("_List", _uow.NewsMainCategoryRepo.GetAll());
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetMainCategoryInfo(int id)
        {
            NewsMainCategory mainCategory = _uow.NewsMainCategoryRepo.Find(id);
            string result = "Name:" + mainCategory.MainCategoryName + ",Order:" + mainCategory.MainCategoryOrder;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            if (!_uow.NewsMainCategoryRepo.HasCategories(id))
            {
                _uow.NewsMainCategoryRepo.DeleteByID(id);
                _uow.SaveChanges();

                return Json(_uow.NewsMainCategoryRepo.GetAll(), JsonRequestBehavior.AllowGet);
            }
            else
                return new HttpStatusCodeResult(4);
        }
	}
}