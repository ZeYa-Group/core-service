using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    internal class UserPackageEntity
    {
        public long Id { get; set; }
        public long PackageId { get; set; }
        public Guid UserId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual PackageTemplateEntity Package { get; set; }
    }
}
