﻿using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ServiceAutomation.Canvas.WebApi.Services
{
    public class AuthOptions
    {
        public const string ISSUER = "AppAuthServer"; 
        public const string AUDIENCE = "AppAuthClient"; 
        private const string KEY = "secretkey!123";
        
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
