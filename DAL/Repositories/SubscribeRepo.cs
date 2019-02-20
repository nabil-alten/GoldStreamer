using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.DomainClasses;

namespace DAL.Repositories
{
    public class SubscribeRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        public SubscribeRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }
        public List<Subscribe> GetAllSubscribes()
        {
            return _context.Set<Subscribe>().Where(b => b.IsSubscribe  == true).ToList();
        }
        public int CheckEmailExists(string Email)
        {
            Subscribe SubscribeEmail =
                _context.Set<Subscribe>()
                        .FirstOrDefault(
                            b => b.Email == Email.Trim() && b.IsSubscribe==true);
            return SubscribeEmail == null ? 0 : 1;
        }
       
       
    }
}