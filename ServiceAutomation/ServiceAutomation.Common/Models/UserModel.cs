using ServiceAutomation.Common.Models;
using System;
using System.Collections.Generic;

namespace ServiceAutomation.Common.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        //public List<Role> Roles { get; set; }

        public RefreshToken RefreshToken { get; set; }

    }
}
