﻿namespace ServiceAutomation.Canvas.WebApi.Constants
{
    public static class Requests
    {
        public static class Home
        {
            public const string GetReferralLink = "GetReferralLink";
            public const string GetAuthAction = "GetAuth";
            public const string GetAction = "GetAction";
        }

        public static class User
        {
            public const string Login = "Login";
            public const string Logout = "Logout";
            public const string Register = "Register";
            public const string Refresh = "Refresh";
            public const string GetUserId = "GetUserId";
        }

        public static class Info
        {
            public const string GetThumbnails = "GetThumbnails";
            public const string GetThumbnailById = "GetThumbnailById";
        }

        public static class Package
        {
            public const string GetPackages = nameof(GetPackages);
            public const string BuyPackage = nameof(BuyPackage);
            public const string GetUserPackage = nameof(GetUserPackage);
        }

        public static class Withdraw
        {
            public const string GetWithdrawHistory = "GetWithdrawHistory";
            public const string MakeWithdraw = "Withdraw";
        }

        public static class Group
        {
            public const string GetTree = "GetTree";
            public const string AddUser = "AddUser";
        }

        public static class VideoTemplate
        {
            public const string GetVideos = "GetVideos";
            public const string GetVideo = "GetVideoById";
        }

        public static class UserProfile
        {
            public const string GetProfileInfo = "GetProfileInfo";
            public const string UploadProfilePhoto = "UploadProfilePhoto";
            public const string UploadProfileInfo = "UploadProfileInfo";
        }
    }
}