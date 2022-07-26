using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.DbSets
{
    public partial class ServiceDbContext
    {
        public virtual DbSet<Object> DbSets { get; set; }
    }
}
