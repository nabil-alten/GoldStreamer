using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using BLL.DomainClasses;

namespace DAL
{
    public class GoldStreamerContext : DbContext
    {
        public GoldStreamerContext()
            : base("GoldStreamerContext") //GoldStreamerContext is the connection string of the DB
        {
            Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<TraderPrices> TradersPrices { get; set; }
        public DbSet<Basket> Basket { get; set; }
        public DbSet<BasketPrices> BasketPrices { get; set; }
        public DbSet<BasketTraders> BasketTraders { get; set; }
        public DbSet<TraderFavorites> TraderFavorates { get; set; }
        public DbSet<FavoriteList> FavorateList { get; set; }
        public DbSet<Users> Users { get; set; }

        public DbSet<Governorate> Governorate { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<QuestionGroup> QuestionGroup { get; set; }

        public DbSet<NewsMainCategory> NewsMainCategory { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<NewsCategory> NewsCategory { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<GlobalPrice> GlobalPrices { get; set; }
        public DbSet<TraderPricesChart> TraderPricesCharts { get; set; }

        public DbSet<Dollar> Dollar { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Trader Indexies
            modelBuilder.Entity<Trader>().Property(e => e.TypeFlag).HasColumnAnnotation
                ("Index", new IndexAnnotation(new IndexAttribute()));
            modelBuilder.Entity<Trader>().Property(e => e.SortOrder).HasColumnAnnotation
                ("Index", new IndexAnnotation(new IndexAttribute()));
            //modelBuilder.Entity<Trader>().Property(e => e.Name).HasColumnAnnotation
            //   ("Index", new IndexAnnotation(new IndexAttribute()));
            //modelBuilder.Entity<Trader>().Property(e => e.Family).HasColumnAnnotation
            //   ("Index", new IndexAnnotation(new IndexAttribute()));

            //TraderPrices Indexies
            modelBuilder.Entity<TraderPrices>().Property(e => e.priceDate).HasColumnAnnotation
                ("Index", new IndexAnnotation(new IndexAttribute()));

            //Basket Indexies
            modelBuilder.Entity<Basket>().Property(e => e.IsDeleted).HasColumnAnnotation
                ("Index", new IndexAnnotation(new IndexAttribute()));

            //BasketPrices Indexies
            modelBuilder.Entity<BasketPrices>().Property(e => e.PriceDate).HasColumnAnnotation
                ("Index", new IndexAnnotation(new IndexAttribute()));

            base.OnModelCreating(modelBuilder);
        }
    }
}