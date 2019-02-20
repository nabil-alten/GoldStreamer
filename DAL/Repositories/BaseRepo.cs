using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repositories
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext db;
        private readonly DbSet<T> table;
        public BaseRepo()
        {
            db = new GoldStreamerContext();
            table = db.Set<T>();
        }
        public BaseRepo(GoldStreamerContext db)
        {
            this.db = db;
            table = db.Set<T>();
        }
        //public IEnumerable<T> ToList()
        //{
        //    return table.ToList();
        //}
        public T Find(object id)
        {
            return table.Find(id);
        }
        public void Add(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            db.Entry(obj).State = EntityState.Modified;
            //table.Attach(obj);
            
        }
        public void Remove(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
        public virtual IEnumerable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return db.Set<T>().Where(predicate);
        }
        public List<T> GetAll()
        {
            return db.Set<T>().ToList();
        }
        public void DeleteAll()
        {
            db.Set<T>().RemoveRange(GetAll());
        }
    }
}