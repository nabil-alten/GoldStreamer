using System;
using System.Collections.Generic;
using System.Linq;
using BLL.DomainClasses;
using DAL.UnitOfWork;

namespace BusinessServices
{
    public class BasketService
    {
        private readonly UnitOfWork _uow;
        public BasketService()
        {
            _uow = new UnitOfWork();
        }
        public List<Basket> List(string sortOrder, int traderId)
        {
            sortOrder = sortOrder ?? "";
            List<Basket> vmBasketLst = _uow.BasketRepo.GetTraderBasketsPrice(traderId);

            foreach(var v in vmBasketLst.Where(b => b.BasketPrices == null))
            {
                v.BasketPrices = new List<BasketPrices>
                {
                    new BasketPrices {BuyPrice = 0, SellPrice = 0, PriceDate = DateTime.Now.Date}
                };
            }

            if(vmBasketLst.Count > 0)
            {
                switch(sortOrder)
                {
                    case "nameDsc":
                        vmBasketLst = vmBasketLst.OrderByDescending(b => b.Name).ToList();
                        break;
                    case "BuyAsc":
                        vmBasketLst = vmBasketLst.OrderBy(b => b.BasketPrices[0].BuyPrice).ToList();
                        break;
                    case "BuyDesc":
                        vmBasketLst = vmBasketLst.OrderByDescending(b => b.BasketPrices[0].BuyPrice).ToList();
                        break;
                    case "SellAsc":
                        vmBasketLst = vmBasketLst.OrderBy(b => b.BasketPrices[0].SellPrice).ToList();
                        break;
                    case "SellDesc":
                        vmBasketLst = vmBasketLst.OrderByDescending(b => b.BasketPrices[0].SellPrice).ToList();
                        break;
                    case "TimeAsc":
                        vmBasketLst = vmBasketLst.OrderBy(b => b.BasketPrices[0].PriceDate).ToList();
                        break;
                    case "TimeDesc":
                        vmBasketLst = vmBasketLst.OrderByDescending(b => b.BasketPrices[0].PriceDate).ToList();
                        break;
                    case "":
                        vmBasketLst = vmBasketLst.OrderBy(b => b.Name).ToList();
                        break;
                }
            }

            return vmBasketLst;
        }
        public void DeleteBasket(int basketId)
        {
            _uow.BasketRepo.DeleteBasket(basketId);
            _uow.BasketTradersRepo.DeleteBasketUsers(basketId);
            _uow.SaveChanges();
        }
        public void SaveAssignedUsers(int basketId, int[] userIds, string basketName)
        {
            _uow.BasketRepo.Find(basketId).Name = basketName;
            _uow.BasketTradersRepo.DeleteBasketUsers(basketId);

            if(userIds != null)
            {
                foreach(var usr in userIds)
                {
                    _uow.BasketTradersRepo.Add(new BasketTraders {BasketId = basketId, TraderId = usr});
                }
            }
            _uow.SaveChanges();
        }
    }
}