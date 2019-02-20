using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BLL.DomainClasses;
using DAL.UnitOfWork;


namespace DAL.Repositories
{
    public class RegionRepo
    {
        DbContext Context = null;
        public RegionRepo(DbContext _DBContext)
        {
            Context = _DBContext;
        }


        public void AddRegion(Region c)
        {

            Context.Set<Region>().Add(c);

        }

        public List<Region> GetAllRegions()
        {
            return Context.Set<Region>().ToList();
        }
        public List<Region> GetCityRegions(int cityId)
        {
            return Context.Set<Region>().Where(c => c.CityID == cityId).ToList();
        }
        public Region FindRegionByCode(string Code)
        {
            return Context.Set<Region>().SingleOrDefault(x => x.Code == Code);
        }

        public Region FindRegionByID(int ID)
        {
            return Context.Set<Region>().SingleOrDefault(x => x.ID == ID);
        }
        public void DeleteAll()
        {
            Context.Set<Region>().RemoveRange(GetAllRegions());
        }

        public void Delete(Region c)
        {
            Context.Set<Region>().Remove(c);
        }

        public void UpdateRegion(Region c)
        {
            Context.Entry(c).State = EntityState.Modified;
        }

        public int IsCodeExist(int? ID, string Code)
        {
            int res = 0;
            if (ID == null)
            {
                var CodeResult = Context.Set<Region>().Where(x => x.Code == Code).ToList();
                res = CodeResult.Count;
            }
            else
            {
                var CodeResult = Context.Set<Region>().Where(x => x.ID != ID && x.Code == Code).ToList();
                res = CodeResult.Count;
            }
            if (res > 0)
            {
                res = 1;
            }
            return res;
        }

        public int IsPrimaryDescExist(int? ID, string PrimaryDesc)
        {
            int res = 0;
            if (ID == null)
            {
                var PrimResult = Context.Set<Region>().Where(x => x.Name == PrimaryDesc).ToList();
                res = PrimResult.Count;
            }
            else
            {
                var PrimResult = Context.Set<Region>().Where(x => x.ID != ID && x.Name == PrimaryDesc).ToList();
                res = PrimResult.Count;
            }
            if (res > 0)
            {
                res = 2;
            }
            return res;
        }

     
        public int ISExist(int? ID, string Code, string PrimaryDesc, bool IsAdd)
        {
            int res = 0;
            if (IsCodeExist(ID, Code) > 0)
            {
                res = 1;
            }

            else if (IsPrimaryDescExist(ID, PrimaryDesc) > 0)
            {
                res = 2;
            }
            

            return res;
        }
   

    }
}

