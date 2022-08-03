using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Schemas.EntityModels
{
    public class CanvasLeadContactEntity
    {
        public int Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid PerantTenantId { get; set; }
        public Guid TenantGroupId { get; set; }

        public string EmailAdress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryPhone { get; set; }
    }
}
