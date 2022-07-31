using Microsoft.EntityFrameworkCore;
using ServiceAutomation.DataAccess.Schemas.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.DbSets
{
    public class ServiceDbContext : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }

        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
