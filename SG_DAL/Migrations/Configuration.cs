namespace SG_DAL.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SG_DAL.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<SG_DAL.Context.SGContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SG_DAL.Context.SGContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            new List<User>
            {
                new User { Ad = "Andrew", Soyad = "Peters", TCKimlik = "10000000001", Email = "info@bilsa.com.tr", Sifre = "12345678" },
                new User { Ad = "Derrek", Soyad = "Campbell", TCKimlik = "10000000002", Email = "info2@bilsa.com.tr", Sifre = "12345678" },
                new User { Ad = "David", Soyad = "Sworf", TCKimlik = "10000000003", Email = "info3@bilsa.com.tr", Sifre = "12345678" }
            }.ForEach(i => context.User.AddOrUpdate(i));

        }
    }
}
