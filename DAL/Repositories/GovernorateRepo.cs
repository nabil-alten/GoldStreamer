using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BLL.DomainClasses;
using DAL.UnitOfWork;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace DAL.Repositories
{
    public class GovernorateRepo
    {
        DbContext _context = null;
        public GovernorateRepo(DbContext dbContext)
        {
            _context = dbContext;
        }


        public void AddGovernorate(Governorate c)
        {

            _context.Set<Governorate>().Add(c);

        }

        public List<Governorate> GetAllGovernorates()
        {
            return _context.Set<Governorate>().ToList();
        }

        public Governorate FindGovernorateByCode(string Code)
        {
            return _context.Set<Governorate>().SingleOrDefault(x => x.Code == Code);
        }

        public Governorate FindGovernorateByID(int ID)
        {
            return _context.Set<Governorate>().SingleOrDefault(x => x.ID == ID);
        }
        public void DeleteAll()
        {
            _context.Set<Governorate>().RemoveRange(GetAllGovernorates());
        }

        public void Delete(Governorate c)
        {
            _context.Set<Governorate>().Remove(c);
        }

        public void UpdateGovernorate(Governorate c)
        {
            _context.Entry(c).State = EntityState.Modified;
        }

        public int IsCodeExist(int? ID, string Code)
        {
            int res = 0;
            if (ID == null)
            {
                var CodeResult = _context.Set<Governorate>().Where(x => x.Code == Code).ToList();
                res = CodeResult.Count;
            }
            else
            {
                var CodeResult = _context.Set<Governorate>().Where(x => x.ID != ID && x.Code == Code).ToList();
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
                var PrimResult = _context.Set<Governorate>().Where(x => x.Name == PrimaryDesc).ToList();
                res = PrimResult.Count;
            }
            else
            {
                var PrimResult = _context.Set<Governorate>().Where(x => x.ID != ID && x.Name == PrimaryDesc).ToList();
                res = PrimResult.Count;
            }
            if (res > 0)
            {
                res = 2;
            }
            return res;
        }

   
        public int IsExist(int? ID, string Code, string PrimaryDesc, string SecondDesc, bool IsAdd)
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
        public bool HasCities(int GovernorateId)
        {
            bool Flag;
            var Cities = _context.Set<City>().Where(c => c.GovernorateID == GovernorateId).ToList();
            if (Cities.Count > 0)
                Flag = true;
            else
                Flag = false;
            return Flag;

        }
    }
}