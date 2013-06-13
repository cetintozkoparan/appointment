using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using SG_DAL.Entities;
using SG_DAL.Context;

namespace SG_DAL.Context
{
    public class SGContext : DbContext
    {
        public SGContext() : base("name=SGContext") { }

        public DbSet<User> User { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<Sinav> Sinav { get; set; }
        public DbSet<SinavOturum> SinavOturum { get; set; }
        public DbSet<SinavGorevli> SinavGorevli { get; set; }
        public DbSet<SinavDurum> SinavDurum { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<SinavOturumOkullari> SinavOturumOkullari { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DatabaseCreatorClass());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SGContext, Migrations.Configuration>());

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<School>().ToTable("School");
            modelBuilder.Entity<SinavOturum>().ToTable("SinavOturum");
            modelBuilder.Entity<Sinav>().ToTable("Sinav");
            modelBuilder.Entity<SinavGorevli>().ToTable("SinavGorevli");
            modelBuilder.Entity<SinavDurum>().ToTable("SinavDurum");
            modelBuilder.Entity<Setting>().ToTable("Setting");
            modelBuilder.Entity<SinavOturumOkullari>().ToTable("SinavOturumOkullari");
        }
    }
}
