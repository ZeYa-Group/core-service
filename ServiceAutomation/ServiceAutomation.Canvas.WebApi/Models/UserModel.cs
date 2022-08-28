using ServiceAutomation.Common.Models;
using ServiceAutomation.DataAccess.Schemas.Enums;
using System;
using System.Collections.Generic;


namespace ServiceAutomation.Common.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string InviteCode { get; set; }
        public long GroupId { get; set; }
        public Country Country { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
