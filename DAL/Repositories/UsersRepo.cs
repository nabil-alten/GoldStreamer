using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainClasses;
using BLL.SecurityClasses;

namespace DAL.Repositories
{
    public class UsersRepo<T> : BaseRepo<T> where T : class
    {
        private readonly GoldStreamerContext _context;
        private readonly ApplicationDbContext _Appcontext=new ApplicationDbContext();
        public UsersRepo(GoldStreamerContext dbc)
            : base(dbc)
        {
            _context = dbc;
        }
       
        public List<Users> GetByTraderId(int TraderId)
        {
            return _context.Set<Users>().Where(x => x.TraderId == TraderId).ToList();
        }
        public List<Users> GetByUserName(string UserName)
        {
            return _context.Set<Users>().Where(x => x.UserName == UserName).ToList();
        }
        public List<Users> Search(string searchTxt, int traderId)
        {
            return _context.Set<Users>().Where(x => x.TraderId == traderId && (x.UserName.ToLower().Contains(searchTxt.ToLower()) || x.Password.ToLower().Contains(searchTxt.ToLower()))).ToList();
        }
        public bool IsUserNameExist(string username)
        {
            bool flag;
            var result = _Appcontext.Set<ApplicationUser>().Where(x => x.UserName == username).ToList();
            if (result.Count > 0)
                flag = true;
            else
                flag = false;
            return flag;
        }
        public bool IsUserOldPasswordExist(int Id, string Passwprd)
        {
            bool flag;
            var result = _context.Set<Users>().Where(x => x.Id == Id && x.Password == Passwprd).ToList();
            if (result.Count > 0)
                flag = true;
            else
                flag = false;
            return flag;
        }
      
        public List<Trader> GetAllTradersAndUsers()
        {

            return _context.Set<Trader>().Where(x=>x.TypeFlag==2 || x.TypeFlag==3).ToList();
        }
        public List<Trader> GetAllTradersAndUsersSearch(string Name, int? TypeFlg)
        {
            if ((TypeFlg == null || TypeFlg == 0)&&(Name!=null&&Name!=""))
            {
                return _context.Set<Trader>().Where(x => (x.TypeFlag == 2 || x.TypeFlag == 3) && (x.Name.Trim().ToLower().Contains(Name.Trim().ToLower()) || x.Family.Trim().ToLower().Contains(Name.Trim().ToLower()) || (x.Name.Trim().ToLower() +" "+ x.Family.Trim().ToLower()).Contains(Name.Trim().ToLower()))).ToList();
            }
            else if ((TypeFlg != null && TypeFlg != 0) && (Name == null || Name == ""))
            {
                return _context.Set<Trader>().Where(x => x.TypeFlag == TypeFlg).ToList();
            }
            else if ((TypeFlg != null && TypeFlg != 0) && (Name != null && Name != ""))
            {
                return _context.Set<Trader>().Where(x => (x.TypeFlag == TypeFlg) || (x.Name.Trim().ToLower().Contains(Name.Trim().ToLower()) || x.Family.Trim().ToLower().Contains(Name.Trim().ToLower()) || (x.Name.Trim().ToLower() + " " + x.Family.Trim().ToLower()).Contains(Name.Trim().ToLower()))).ToList();
            }
            else 
            {
                return _context.Set<Trader>().ToList();
            }
        }
        public List<ApplicationUser> BlockUnBolckList()
        {

            return _Appcontext.Set<ApplicationUser>().ToList();
        }
        public List<ApplicationUser> BlockUnBolckSearch(string Loginname)
        {
            if(Loginname==null||Loginname=="")
            {
                return _Appcontext.Set<ApplicationUser>().ToList();
            }

            return _Appcontext.Set<ApplicationUser>().Where(x=>x.UserName.ToLower().Contains(Loginname.Trim().ToLower())).ToList();
        }

    }
}
