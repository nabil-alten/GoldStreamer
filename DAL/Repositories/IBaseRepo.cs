namespace DAL.Repositories
{
    public interface IBaseRepo<T> where T : class
    {
        //IEnumerable<T> ToList();
        T Find(object id);
        void Add(T obj);
        void Update(T obj);
        void Remove(object id);
        void SaveChanges();
    }
}