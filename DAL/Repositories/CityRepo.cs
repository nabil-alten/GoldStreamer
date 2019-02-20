using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using System.Data.Entity;

namespace DAL.Repositories
{
    public class CityRepo
    {
        DbContext Context = null;
        public CityRepo(DbContext _DBContext)
        {
            Context = _DBContext;
        }


        public void AddCity(City c)
        {

            Context.Set<City>().Add(c);

        }

        public List<City> GetAllCities()
        {
            return Context.Set<City>().ToList();
        }

        public List<City> GetGovCities(int govId)
        {
            return Context.Set<City>().Where(c=>c.GovernorateID==govId).ToList();
        }

        public City FindCityByCode(string code)
        {
            return Context.Set<City>().SingleOrDefault(x => x.Code == code);
        }

        public City FindCityByID(int id)
        {
            return Context.Set<City>().SingleOrDefault(x => x.ID == id);
        }
        public void DeleteAll()
        {
            Context.Set<City>().RemoveRange(GetAllCities());
        }

        public void Delete(City c)
        {
            Context.Set<City>().Remove(c);
        }

        public void UpdateCity(City c)
        {
            Context.Entry(c).State = EntityState.Modified;
        }

        public int IsCodeExist(int? id, string code)
        {
            int res = 0;
            if (id == null)
            {
                var codeResult = Context.Set<City>().Where(x => x.Code == code).ToList();
                res = codeResult.Count;
            }
            else
            {
                var codeResult1 = Context.Set<City>().Where(x => x.ID != id && x.Code == code).ToList();
                res = codeResult1.Count;
            }
            if (res > 0)
            {
                res = 1;
            }
            return res;
        }

        public int IsPrimaryDescExist(int? id, string primaryDesc)
        {
            int res = 0;
            if (id == null)
            {
                var primResult = Context.Set<City>().Where(x => x.Name == primaryDesc).ToList();
                res = primResult.Count;
            }
            else
            {
                var primResult1 = Context.Set<City>().Where(x => x.ID != id && x.Name == primaryDesc).ToList();
                res = primResult1.Count;
            }
            if (res > 0)
            {
                res = 2;
            }
            return res;
        }

       
        public int IsExist(int? id, string code, string primaryDesc, string secondDesc, bool isAdd)
        {
            int res = 0;
            if (IsCodeExist(id, code) > 0)
            {
                res = 1;
            }

            else if (IsPrimaryDescExist(id, primaryDesc) > 0)
            {
                res = 2;
            }
           

            return res;
        }
        //public bool CheckRelatedRegionData(int id)
        //{
        //    bool flag;
        //    var result = (from x in Context.Region
        //                  where x.CityID == id
        //                  select x).ToList();

        //    if (result.Count > 0)
        //        flag = true;
        //    else
        //    {

        //        flag = false;
        //    }
        //    return flag;

        //}
     
    }
}