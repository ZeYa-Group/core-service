using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class UserContactEntity
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string IdentityCode { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
