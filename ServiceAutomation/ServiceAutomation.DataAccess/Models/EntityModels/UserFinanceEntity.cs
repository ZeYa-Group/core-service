using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class UserFinanceEntity : Entity
    {
        public Guid UserId { get; set; }
        public decimal AllTimeIncome { get; set; }
        public decimal AvailableForWithdrawal { get; set; }
        public decimal AwaitingAccrual { get; set; }

        public virtual UserEntity User { get; set; }
    }
}