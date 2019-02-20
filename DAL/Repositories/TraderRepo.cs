using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class TraderRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public TraderRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }

        public Trader FindTraderById(int id)
        {
            return _context.Set<Trader>().AsNoTracking().SingleOrDefault(x => x.ID == id);
        }

        public Trader FindTraderByName (string Name)
        {
            return _context.Set<Trader>().AsNoTracking().SingleOrDefault(x => x.Name == Name);
        }
        public List<Trader> GetRegisteredTrader(int userType)
        {
            return _context.Set<Trader>().Where(t => t.TypeFlag == userType && t.IsActive).ToList();
        }
        public void DeleteList(int initObjId)
        {
            _context.Set<Trader>().RemoveRange(_context.Set<Trader>().Where(t => t.ID >= initObjId).ToList());
        }
        public int CheckEmailExists(string email, int traderId)
        {
            Trader name =
              _context.Set<Trader>().AsNoTracking()
                      .FirstOrDefault(
                          t => t.Email == email.Trim().ToString() //&& t.IsActive == true 
                          && (t.ID != traderId));

            if (name == null)
                return 0;
            if (name.ID == traderId)
                return 0;

            return 1;
        }
        public int CheckOrderExists(int order, int traderId)
        {
            Trader name =
              _context.Set<Trader>().AsNoTracking()
                      .FirstOrDefault(
                          t => t.SortOrder == order //&& t.IsActive == true 
                          );

            if (name == null)
                return 0;
            if (name.ID == traderId)
                return 0;

            return 1;
        }
        public List<Trader> SearchSpecificType(string name, int traderType)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var x = (from c in _context.Set<Trader>()
                         where c.Name.Contains(name)
                         && c.TypeFlag == traderType
                         select c).ToList();
                return x;
            }
            return null;
        }
        public List<Trader> GetAllByType(int userType)
        {
            return _context.Set<Trader>().Where(t => t.TypeFlag == userType).ToList();
        }

        public int CheckNameExists(Trader traderObj)
        {
            Trader name =
             _context.Set<Trader>().AsNoTracking()
                     .FirstOrDefault(
                         t => t.Name == traderObj.Name.Trim().ToString() && t.Family.ToString() == traderObj.Family.Trim().ToString() && (t.ID != traderObj.ID || traderObj.ID == 0)
                         );

            if (name == null)
                return 0;
            if (name.ID == traderObj.ID)
                return 0;

            return 1;
        }

        public int CheckMobileExists(Trader traderObj)
        {
            Trader name =
             _context.Set<Trader>().AsNoTracking()
                     .FirstOrDefault(
                         t => t.Mobile == traderObj.Mobile.Trim().ToString()// && t.Family == traderObj.Family
                          && (t.ID != traderObj.ID || traderObj.ID == 0));

            if (name == null)
                return 0;
            if (name.ID == traderObj.ID)
                return 0;

            return 1;
        }

        public int CheckShopNameExists(Trader traderObj)
        {
            Trader name =
             _context.Set<Trader>().AsNoTracking()
                     .FirstOrDefault(
                         t => t.ShopName == traderObj.ShopName.Trim().ToString()// && t.Family == traderObj.Family
                          && (t.ID != traderObj.ID || traderObj.ID == 0));

            if (name == null)
                return 0;
            if (name.ID == traderObj.ID)
                return 0;

            return 1;
        }

        public int CheckPhoneExists(Trader traderObj)
        {
            Trader name =
             _context.Set<Trader>().AsNoTracking()
                     .FirstOrDefault(
                         t => t.Phone == traderObj.Phone.Trim().ToString()// && t.Family == traderObj.Family
                         && (t.ID != traderObj.ID || traderObj.ID == 0));

            if (name == null)
                return 0;
            if (name.ID == traderObj.ID)
                return 0;

            return 1;
        }
        public int GetNextTraderOrder()
        {
            return _context.Set<Trader>().Max(t => t.SortOrder)+1;
        }



    }
}