using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.DbContexts
{
    public class PotgreSqlContext : ServiceDbContext
    {
        public PotgreSqlContext()
        {

        }

        public PotgreSqlContext(DbContextOptions<PotgreSqlContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //OnPostgreModelCreatingMxtr(modelBuilder);
            //OnPostgreModelCreatingIntegrations(modelBuilder);
            //OnPostgreModelCreatingMessagesGears(modelBuilder);
            //OnPostgreModelCreatingCanvas(modelBuilder);
        }
    }
}
