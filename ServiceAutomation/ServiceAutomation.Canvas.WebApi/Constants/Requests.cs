﻿namespace ServiceAutomation.Canvas.WebApi.Constants
{
    public static class Requests
    {
        public static class Home
        {
            public const string GetAction = "Get";
            public const string GetAuthAction = "GetAuth";
        }

        public static class User
        {
            public const string Login = "Login";
            public const string Logout = "Logout";
            public const string Register = "Register";
            public const string Refresh = "Refresh";
        }

        public static class Info
        {
            public const string GetThumbnails = "GetThumbnails";
        }
    }
}
