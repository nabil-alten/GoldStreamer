using BLL.DomainClasses;
using BLL.SecurityClasses;
using DAL;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoldStreamWebApp.Tests
{
    [TestClass]
   public class UsersTest
    {
        UnitOfWork uow = null;
         private  ApplicationDbContext UsersContext = new ApplicationDbContext();
        [TestInitialize]
        public void Setup()
        {
            uow = new UnitOfWork(new GoldStreamerContext(), DropDB: false);
        }
        private Trader AddTrader(String name, String uname)
        {

            Trader traderObj = new Trader();
            traderObj.Name = name;
            traderObj.Gender = true;
            traderObj.Governorate = 1;
            traderObj.City = 1;
            traderObj.District = 1;
            traderObj.TypeFlag = 1;
            traderObj.SortOrder = 1;

            uow.TraderRepo.Add(traderObj);
            uow.SaveChanges();
            return traderObj;

        }
        private void DeleteAddedTraders()
        {
            uow.TradePricesRepo.DeleteAll();
            uow.BasketPricesRepo.DeleteAll();
            uow.BasketTradersRepo.DeleteAll();
            uow.BasketRepo.DeleteAll();

            uow.FavorateListRepo.DeleteAll();
            uow.TraderFavRepo.DeleteAll();

            uow.UsersRepo.DeleteAll();

            uow.TraderRepo.DeleteList(26);
            uow.SaveChanges();
        }
        private Users AddUser(int traderId, String userName)
        {
            Users UserObj = new Users();
            UserObj.UserName = userName;
            UserObj.TraderId = traderId;
            UserObj.Password = "TestPassword";
            UserObj.IsActive = true;
            UserObj.NeedReset = true;
            uow.UsersRepo.Add(UserObj);
           
            return UserObj;
        }
        private void DeleteAllUsers()
        {
            uow.UsersRepo.DeleteAll();
            DeleteAddedTraders();
        }
        [TestMethod]
        public void CanAddTraderUser()
        {
            DeleteAllUsers();
            Trader obj = AddTrader("تاجر20","Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            List<Users> lstUsers = uow.UsersRepo.GetByTraderId(obj.ID);
            Assert.AreEqual(2, lstUsers.Count());
        }
        [TestMethod]
        public void CanSearchTraderUser()
        {
            DeleteAllUsers();
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            List<Users> lstUsers = uow.UsersRepo.Search("Dina",obj.ID);
            Assert.AreEqual(2, lstUsers.Count());
        }
        [TestMethod]
        public void CanSearchOneTraderUser()
        {
            DeleteAllUsers();
           
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            List<Users> lstUsers = uow.UsersRepo.Search("Mostafa", obj.ID);
            Assert.AreEqual(1, lstUsers.Count());
        }
        [TestMethod]
        public void CantraderUserIsExists()
        {
            DeleteAllUsers();
            
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            bool check = uow.UsersRepo.IsUserNameExist("DinaMostafa");
            Assert.AreEqual(true, check);
        }
        [TestMethod]
        public void CantraderUserIsNotExists()
        {
            DeleteAllUsers();
           
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            bool check = uow.UsersRepo.IsUserNameExist("DinaMostafa123");
            Assert.AreEqual(false, check);
        }
        [TestMethod]
        public void CanDeactivateTraderUser()
        {
            DeleteAllUsers();
           
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            userObj1.IsActive = false;
            uow.UsersRepo.Update(userObj1);
            uow.SaveChanges();
            Users User = uow.UsersRepo.Find(userObj1.Id);
            Assert.AreEqual(false, User.IsActive);
        }
        [TestMethod]
        public void CanactivateTraderUser()
        {
            DeleteAllUsers();
          
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            userObj1.IsActive = false;
            uow.UsersRepo.Update(userObj1);
            uow.SaveChanges();
            userObj1.IsActive = true;
            uow.UsersRepo.Update(userObj1);
            uow.SaveChanges();
            Users User = uow.UsersRepo.Find(userObj1.Id);
            Assert.AreEqual(true, User.IsActive);
        }
        [TestMethod]
        public void CanGenrateTraderUserpassword()
        {
            DeleteAllUsers();
          
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            string OldPass = userObj1.Password;
            userObj1.Password = "TestPassword2";
            uow.UsersRepo.Update(userObj1);
            uow.SaveChanges();
            Users User = uow.UsersRepo.Find(userObj1.Id);
            Assert.AreNotEqual(OldPass, User.Password);
        }

        [TestMethod]
        public void UserOldPasswordNotExist()
        {
            DeleteAllUsers();
          
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            bool check = uow.UsersRepo.IsUserOldPasswordExist(userObj1.Id, "test");
            Assert.AreEqual(false, check);
        }
        [TestMethod]
        public void CheckUserOldPasswordExist()
        {
            DeleteAllUsers();
            
            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            bool check = uow.UsersRepo.IsUserOldPasswordExist(userObj1.Id, "TestPassword");
            Assert.AreEqual(true, check);
        }
         [TestMethod]
        public void CanResetTraderUserpassword()
        {
            DeleteAllUsers();

            Trader obj = AddTrader("تاجر20", "Tagerr");
            Users userObj1 = AddUser(obj.ID, "DinaMostafa");
            Users userObj2 = AddUser(obj.ID, "DinaAhmed");
            uow.SaveChanges();
            userObj1.Password = "TestPassword2";
            uow.UsersRepo.Update(userObj1);
            uow.SaveChanges();
            Users User = uow.UsersRepo.Find(userObj1.Id);
            Assert.AreEqual("TestPassword2", User.Password);
        }
         [TestMethod]
         public void canBlockUnBolckSearch()
         {
             List<ApplicationUser> listUsers = UsersContext.Set<ApplicationUser>().Where(r => r.UserName != "Admin").ToList();
             UsersContext.Set<ApplicationUser>().RemoveRange(listUsers);
             UsersContext.SaveChanges();

             ApplicationUser c = new ApplicationUser();
             c.Id = Guid.NewGuid().ToString();
             c.UserName = "dina mostafa ahmed";
             c.PasswordHash = "AFcluN5/79TMVoKuh/oFBffZJ3ufB2EZ/o1LAQHER4+wjzoNswFPxzE9wT5wpGbRmw==";
             c.SecurityStamp = "f0d290ff-982b-41b4-979b-744531cc72c2";
             c.TraderId = 21;
             c.IsActive = true;
             c.NeedReset = false;
             c.IsVerified = true;
             c.TokenCreationDate = DateTime.Now;

             ApplicationUser c2 = new ApplicationUser();
             c2.Id = Guid.NewGuid().ToString();
             c2.UserName = "ahmed mohamed mahmoud";
             c2.PasswordHash = "AFcluN5/79TMVoKuh/oFBffZJ3ufB2EZ/o1LAQHER4+wjzoNswFPxzE9wT5wpGbRmw==";
             c2.SecurityStamp = "f0d290ff-982b-41b4-979b-744531cc72c2";
             c2.TraderId = 11;
             c2.IsActive = true;
             c2.NeedReset = false;
             c2.IsVerified = true;
             c2.TokenCreationDate = DateTime.Now;

             UsersContext.Set<ApplicationUser>().Add(c);
             UsersContext.Set<ApplicationUser>().Add(c2);
             UsersContext.SaveChanges();

             List<ApplicationUser> lst = uow.UsersRepo.BlockUnBolckList();
             Assert.AreEqual(3, lst.Count());

         }
         [TestMethod]
         public void canGetAllTradersAndUsers()
         {
             List<Trader> lst = uow.UsersRepo.GetAllTradersAndUsers();
             Assert.AreEqual(15, lst.Count());

         }
         [TestMethod]
         public void canGetAllTradersAndUsersSearch()
         {
             CanAddTraderUser();

             List<Trader> lst = uow.UsersRepo.GetAllTradersAndUsersSearch("تاجر",2);
             Assert.AreEqual(21, lst.Count());
         }
    }
}
