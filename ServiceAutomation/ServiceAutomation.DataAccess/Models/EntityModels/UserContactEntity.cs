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
        public string Name { get; set; }
        //все данные из паспорта + телефон 


        public virtual UserEntity User { get; set; }
    }
}
