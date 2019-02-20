using System;
using System.Collections.Generic;
using System.Data.Entity;
using BLL.DomainClasses;
using BLL.SecurityClasses;
using Microsoft.AspNet.Identity;

namespace DAL
{
    public class GoldStreamerInitializer : DropCreateDatabaseIfModelChanges<GoldStreamerContext>
    //DropCreateDatabaseAlways 
    {
        protected override void Seed(GoldStreamerContext context)
        {
            SetSecuritySeed();
            var Govs = new List<Governorate>
            {
                new Governorate {Code = "ca", Name = "Cairo"},
                new Governorate {Code = "alx", Name = "Alex"},
                new Governorate {Code = "gz", Name = "Giza"}
            };
            Govs.ForEach(x => context.Governorate.Add(x));
            context.SaveChanges();

            var city = new List<City>
            {
                new City{GovernorateID = 1,Code="c",Name = "Cairo"},
                new City{GovernorateID = 2,Code="alx",Name = "Alex"},
                new City{GovernorateID = 3,Code="g",Name = "Giza"}
            
            };
            city.ForEach(x => context.City.Add(x));
            context.SaveChanges();

            var region = new List<Region>
            {
                new Region{CityID =  1,Code = "nc",Name = "Nasr city"},
                new Region{CityID = 1,Code = "ab",Name = "Abbaseya"},
                new Region{CityID = 2,Code = "sb",Name = "Sedy beshr"},
                new Region{CityID = 2,Code = "st",Name = "Stanley"},
                new Region{CityID = 3,Code = "ag",Name = "Agouza"},
                new Region{CityID = 3,Code = "fs",Name = "Faisal"}
            };
            region.ForEach(x => context.Region.Add(x));
            context.SaveChanges();

            var traders = new List<Trader>
            {
                new Trader
                {
                    Name = "تاجر رئيسىA",
                    Family = "عائله",
                    Governorate = Govs[0].ID,
                    City = city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 1,
                    IsActive = true,Mobile = "01564756756756",Email = "mohamed25@techvision-eg.com"//,ReEmail = "mohamed25@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىB",
                   Family = "عائله", 
                    Governorate = Govs[0].ID,
                    City = city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 2,
                    IsActive = true,Mobile = "011100016156",Email = "mohamed1@techvision-eg.com"//,ReEmail = "mohamed1@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىC",
                  Family = "عائله",
                   Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 3,
                    IsActive = true,Mobile = "011140001156",Email = "mohamed2@techvision-eg.com"//,ReEmail = "mohamed2@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىD",
                    Family = "",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 4,
                    IsActive = true,Mobile = "011100701156",Email = "mohamed3@techvision-eg.com"//,ReEmail = "mohamed3@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىE",
                    Family = "",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 5,
                    IsActive = true,Mobile = "0111000751156",Email = "mohamed4@techvision-eg.com"//,ReEmail = "mohamed4@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىF",
                    Family = "",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 6,
                    IsActive = true,Mobile = "01345110001156",Email = "mohamed5@techvision-eg.com"//,ReEmail = "mohamed5@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىG",
                    Family = "",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 7,
                    IsActive = true,Mobile = "01110543001156",Email = "mohamed6@techvision-eg.com"//,ReEmail = "mohamed6@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىH",
                    Family = "df",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 8,
                    IsActive = true,Mobile = "01110345001156",Email = "mohamed7@techvision-eg.com"//,ReEmail = "mohamed7@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىI",
                    Family = "gf",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 9,
                    IsActive = true,Mobile = "011103453001156",Email = "mohamed8@techvision-eg.com"//,ReEmail = "mohamed8@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجر رئيسىJ",
                    Family = "gfsdf",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 1,
                    SortOrder = 10,
                    IsActive = true,Mobile = "0111002301156",Email = "mohamed9@techvision-eg.com"//,ReEmail = "mohamed9@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرa",
                   Family = "عائله",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "0321110001156",Email = "mohamed10@techvision-eg.com"//,ReEmail = "mohamed10@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرb",
                   Family = "عائله",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "0158110001156",Email = "mohamed11@techvision-eg.com"//,ReEmail = "mohamed11@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرc",
                    Family = "cf",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "011105675001156",Email = "mohamed12@techvision-eg.com"//,ReEmail = "mohamed12@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرd",
                    Family = "عائله",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "01123110001156",Email = "mohamed13@techvision-eg.com"//,ReEmail = "mohamed13@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرe",
                    Family = "ef",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "01110001155466",Email = "mohamed14@techvision-eg.com"//,ReEmail = "mohamed14@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرf",
                    Family = "f",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "01110001234156",Email = "mohamed15@techvision-eg.com"//,ReEmail = "mohamed15@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرg",
                    Family = "gf",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "01411012001156",Email = "mohamed16@techvision-eg.com"//,ReEmail = "mohamed16@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرh",
                    Family = "hf",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "0141110001156",Email = "mohamed17@techvision-eg.com"//,ReEmail = "mohamed17@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرi",
                    Family = "if",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "01115620001156",Email = "mohamed18@techvision-eg.com"//,ReEmail = "mohamed18@techvision-eg.com"
                },
                new Trader
                {
                    Name = "تاجرj",
                    Family = "jf",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 2,
                    SortOrder = 0,
                    IsActive = true,Mobile = "01110001654156",Email = "mohamed19@techvision-eg.com"//,ReEmail = "mohamed19@techvision-eg.com"
                },
                new Trader
                {
                    Name = "مستخدم",
                    Family = "f",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 3,
                    SortOrder = 0,
                    IsActive = true,Mobile = "011100011342556",Email = "mohamed20@techvision-eg.com"//,ReEmail = "mohamed20@techvision-eg.com"
                },
                new Trader
                {
                    Name = "مستخدمx",
                    Family = "xf",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 3,
                    SortOrder = 0,
                    IsActive = true,Mobile = "0111000113456",Email = "mohamed21@techvision-eg.com"//,ReEmail = "mohamed21@techvision-eg.com"
                },
                new Trader
                {
                    Name = "مستخدمz",
                    Family = "zf",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 3,
                    SortOrder = 0,
                    IsActive = true,Mobile = "0111000115546",Email = "mohamed22@techvision-eg.com"//,ReEmail = "mohamed22@techvision-eg.com"
                },
                new Trader
                {
                    Name = "مستخدمd",
                    Family = "df",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 3,
                    SortOrder = 0,
                    IsActive = true,Mobile = "0111000115656",Email = "mohamed23@techvision-eg.com"//,ReEmail = "mohamed23@techvision-eg.com"
                },
                new Trader
                {
                    Name = "مستخدمe",
                    Family = "ef",
                    Governorate = Govs[0].ID,
                    City= city[0].ID,
                    District = region[0].ID,
                    Gender =true,
                    TypeFlag = 3,
                    SortOrder = 0,
                    IsActive = true,Mobile = "0111000221156",Email = "mohamed24@techvision-eg.com"//,ReEmail = "mohamed24@techvision-eg.com"
                }
            };
            //traders.ForEach(x => context.Traders.Add(x));
            //context.SaveChanges();
            foreach (var x in traders)
            {
                context.Traders.Add(x);
                context.SaveChanges();
            }

            //some test baskets - will be deleted later
            var baskets = new List<Basket>
            {
                new Basket {BasketOwner = 1, Name = "سلة الفيوم - تاجر رئيسى1", IsDeleted = false},
                new Basket {BasketOwner = 2, Name = "سلة اسوان - تاجر رئيسى2", IsDeleted = false},
                new Basket {BasketOwner = 3, Name = "سلة المنصورة - تاجر رئيسى3", IsDeleted = false},
                new Basket {BasketOwner = 4, Name = "سلة الاقصر - تاجر رئيسى4", IsDeleted = false},
                new Basket {BasketOwner = 5, Name = "سلة حلوان - تاجر رئيسى5", IsDeleted = false}
            };
            baskets.ForEach(x => context.Basket.Add(x));
            context.SaveChanges();

            var mainCat = new List<NewsMainCategory>
            {
                new NewsMainCategory{MainCategoryName = "محلى",MainCategoryOrder = 1},
                new NewsMainCategory{MainCategoryName = "عالمى",MainCategoryOrder = 2}
            };
            mainCat.ForEach(x => context.NewsMainCategory.Add(x));
            context.SaveChanges();
            var cat = new List<NewsCategory>{
                    new NewsCategory{CategoryName = "أخبار القاهره",CategoryOrder = 1,MainCategoryId =1},
                    new NewsCategory{CategoryName = "أخبار الاسكندريه",CategoryOrder =2,MainCategoryId =1},
                    new NewsCategory{CategoryName = "نيويورك",CategoryOrder = 1,MainCategoryId =2},
                    new NewsCategory{CategoryName = "انجلترا",CategoryOrder = 2,MainCategoryId =2}
                
                };
            cat.ForEach(x => context.NewsCategory.Add(x));
            context.SaveChanges();


            Dollar dollar = new Dollar() 
            {
                SellPrice = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["DollarSellPrice"].ToString()),
                BuyPrice = decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["DollarBuyPrice"].ToString()),
                CaptureDate = DateTime.Now,
            };
            context.Dollar.Add(dollar);
            context.SaveChanges();
        }

        private void SetSecuritySeed()
        {
            AddAllRoles();
            AddAdminUserData();
        }
        private bool AddAdminUserData()
        {
            bool success = true;
            try
            {
                var idManager = new IdentityManager();
                PasswordHasher hasher = new PasswordHasher();
                ApplicationUser userObj = new ApplicationUser();
                userObj.UserName = "admin";
                userObj.PasswordHash = hasher.HashPassword("123456");
                userObj.IsActive = true;
                userObj.NeedReset = false;
                userObj.TokenCreationDate = DateTime.Now;

                userObj.IsVerified = true;
                idManager.CreateUser(userObj, "123456");
                idManager.AddUserToRole(userObj.Id, "Admin");
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        private bool AddAllRoles()
        {
            bool success = true;
            try
            {
                var idManager = new IdentityManager();
                idManager.CreateRole("Admin", "Admin");
                idManager.CreateRole("CanManageFavoriteList", "المفضلة");
                idManager.CreateRole("CanViewFavPrices", "إستعراض عروض المفضلة");
                idManager.CreateRole("CanManageTraderPrices", "تحديث الأسعار");
                idManager.CreateRole("CanManageBasket", "سلة التجار");
                idManager.CreateRole("CanViewPrices", "عروض الأسعار");
                idManager.CreateRole("CanViewTraderPricesHistory", "أسعار التاجر تاريخى");
                idManager.CreateRole("CanManageBasketPrices", "تحديث أسعار السلة");
                idManager.CreateRole("CanViewBasketPricesHistory", " أسعار السلة تاريخى");
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        private void AddRolePermission()
        {

        }
    }
}