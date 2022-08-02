using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceAutomation.Canvas.WebApi.Models.RequestsModels
{
    public class RegisterRequestModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<string> Roles { get; set; }
    }
}
