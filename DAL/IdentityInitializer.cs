using System;
using System.Data.Entity.Migrations;

namespace DAL
{
    public class IdentityInitializer : DbMigrationsConfiguration<ApplicationDbContext>
    //DropCreateDatabaseAlways 
    {
        public IdentityInitializer()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}