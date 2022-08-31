using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class TenantGroupEntity
    {
        public long Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public long ParentId { get; set; }
        public TenantGroupEntity Parent { get; set; }

        public ICollection<TenantGroupEntity> ChildGroups { get; set; }
        public virtual UserEntity OwnerUser { get; set; }
    }
}
