using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Models.EntityModels
{
    public class ProfilePhotoEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Byte[] Data { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
