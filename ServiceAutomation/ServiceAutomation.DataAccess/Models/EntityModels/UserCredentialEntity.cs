using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class UserCredentialEntity
    {
        public long Id { get; set; }
        public long CredentialId {get; set;}
        public Guid UserId { get; set; }

        public virtual CredentialEntity Credential { get; set; }
        public virtual UserEntity User { get; set; }
    }
}