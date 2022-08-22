using ServiceAutomation.DataAccess.Schemas.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class WithdrawTransactionEntity
    {
        public long Id { get; set; }
        public long CredentialId { get; set; }
        public Guid UserId { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual CredentialEntity Credential { get; set; }
    }
}
