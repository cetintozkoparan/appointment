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
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DatabaseCreatorClass());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SGContext, Migrations.Configuration>());

            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
