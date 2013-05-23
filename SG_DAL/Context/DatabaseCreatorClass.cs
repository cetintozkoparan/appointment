using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_DAL.Context
{
    public class DatabaseCreatorClass : IDatabaseInitializer<SGContext>
    {
        public void InitializeDatabase(SGContext context)
        {
            if (context.Database.Exists())
            {
                if (!context.Database.CompatibleWithModel(true))
                {
                    context.Database.Delete();
                    context.Database.Create();
                }
            }
            else
            {
                context.Database.Create();
            }
        }
    }
}
