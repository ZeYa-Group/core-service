using ServiceAutomation.DataAccess.Models.Enums;
using ServiceAutomation.DataAccess.Schemas.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Country Country { get; set; }
        public string PersonalReferral { get; set; }
        public string InviteReferral { get; set; }
        public long GroupId { get; set; }
        //public Role Role { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<CredentialEntity> Credentionals { get; set; }
        public virtual TenantGroupEntity Group { get; set; }
    }
}
