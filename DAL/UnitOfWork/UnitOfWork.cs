using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.DomainClasses;
using DAL.Repositories;
using System.Diagnostics;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private GoldStreamerContext entities = null;
        public TraderRepo<Trader> TraderRepo { get; set; }
        public TraderPricesRepo<TraderPrices> TradePricesRepo { get; set; }
        public PricerRepo<Prices> PriceViewerRepo { get; set; }
        public BasketRepo<Basket> BasketRepo { get; set; }
        public BasketPricesRepo<BasketPrices> BasketPricesRepo { get; set; }
        public BasketTradersRepo<BasketTraders> BasketTradersRepo { get; set; }
        public TraderFavRepo<TraderFavorites> TraderFavRepo { get; set; }
        public FavoriteListRepo<FavoriteList> FavorateListRepo { get; set; }
        public UsersRepo<Users> UsersRepo { get; set; }

        public GovernorateRepo GovernorateRepo { get; set; }
        public CityRepo CityRepo { get; set; }
        public RegionRepo RegionRepo { get; set; }
        public RolePermissionRepo<RolePermission> RolePermissionRepo { get; set; }
        public FeedbackRepo<Feedback> FeedbackRepo { get; set; }
        public QuestionGroupRepo<QuestionGroup> QuestionGroupRepo { get; set; }
        public QuestionRepo<Question> QuestionRepo { get; set; }

        public NewsMainCategoryRepo<NewsMainCategory> NewsMainCategoryRepo { get; set; }
        public NewsCategoryRepo<NewsCategory> NewsCategoryRepo { get; set; }
        public NewsRepo<News> NewsRepo { get; set; }
        public TraderPricesChartRepo<TraderPricesChart> TraderPricesChartRepo { get; set; }
        public SubscribeRepo<Subscribe> SubscribeRepo { get; set; }

        //public EditProfileRepo<Users> EditProfile { get; set; }
        public GlobalPriceRepo<GlobalPrice> GlobalPriceRepo { get; set; }

        public DollarRepo<Dollar> DollarRepo { get; set; }

        public UnitOfWork()
            : this(null, false)
        {

        }

        public UnitOfWork(GoldStreamerContext e, bool DropDB = false)
        {
            if (DropDB)
                DropDatabase(e);
            if (e != null)
                entities = e;
            else
                entities = new GoldStreamerContext();

            TraderRepo = new TraderRepo<Trader>(this.entities);
            TradePricesRepo = new TraderPricesRepo<TraderPrices>(this.entities);
            PriceViewerRepo = new PricerRepo<Prices>(this.entities);
            BasketRepo = new BasketRepo<Basket>(this.entities);
            BasketPricesRepo = new BasketPricesRepo<BasketPrices>(this.entities);
            BasketTradersRepo = new BasketTradersRepo<BasketTraders>(this.entities);
            TraderFavRepo = new TraderFavRepo<TraderFavorites>(this.entities);
            FavorateListRepo = new FavoriteListRepo<FavoriteList>(this.entities);
            UsersRepo = new UsersRepo<Users>(this.entities);
            GovernorateRepo = new GovernorateRepo(this.entities);
            CityRepo = new CityRepo(this.entities);
            RegionRepo = new RegionRepo(this.entities);
            RolePermissionRepo = new RolePermissionRepo<RolePermission>(this.entities);
            FeedbackRepo = new FeedbackRepo<Feedback>(this.entities);
            QuestionGroupRepo = new QuestionGroupRepo<QuestionGroup>(this.entities);
            QuestionRepo = new QuestionRepo<Question>(this.entities);
            SubscribeRepo = new SubscribeRepo<Subscribe>(this.entities);
            NewsMainCategoryRepo = new NewsMainCategoryRepo<NewsMainCategory>(this.entities);
            NewsCategoryRepo = new NewsCategoryRepo<NewsCategory>(this.entities);
            NewsRepo = new NewsRepo<News>(this.entities);
            GlobalPriceRepo = new GlobalPriceRepo<GlobalPrice>(entities);
            TraderPricesChartRepo = new TraderPricesChartRepo<TraderPricesChart>(entities);
            DollarRepo = new DollarRepo<Dollar>(entities);
        }

        public void StartLoggin()
        {
            entities.Database.Log = Console.WriteLine;
        }
        public void StopLoggin()
        {
            entities.Database.Log = null;
        }
        private static void DropDatabase(GoldStreamerContext e)
        {
            e.Database.Delete();
        }


        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public IBaseRepo<T> Repository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IBaseRepo<T>;
            }
            IBaseRepo<T> repo = new BaseRepo<T>(entities);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        public void SaveChanges()
        {
            entities.SaveChanges();
        }

        private bool disposed/* = false*/;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}