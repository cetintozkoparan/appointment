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

            //context.User.AddOrUpdate(c => c.TCKimlik,
            //  new User { Ad = "System", Soyad = "Yöneticisi", TCKimlik = 10000000001, Email = "info@bilsa.com.tr", Sifre = "12345678", IsAdmin = true, IsDeleted = false, Rol = (int)SG_DAL.Enums.EnumRol.yonetici }
            //);

            //context.Setting.AddOrUpdate(c => c.SalonPersonelSayisi,
            //  new Setting { SalonPersonelSayisi = 2, GenelBasvuru = true, GozetmenSiralama1 = 0, GozetmenSiralama2 = 0, GozetmenSiralama3 = 0 }
            //);
        }
    }
}
