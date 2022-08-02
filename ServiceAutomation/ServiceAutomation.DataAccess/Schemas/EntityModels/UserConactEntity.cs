﻿using ServiceAutomation.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Schemas.EntityModels
{
    public class UserContactEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        //public List<Role> Roles { get; set; }

        //public RefreshToken RefreshToken { get; set; }
    }
}
