namespace ServiceAutomation.Canvas.WebApi.Constants
{
    public static class Requests
    {
        public static class Home
        {
            public const string GetReferral = "GetReferral";
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
            public const string GetThumbnailById = "GetThumbnailById";
        }

        public static class PackageTemplate
        {
            public const string GetPackTemplates = "GetPackTemplates";
            public const string BuyPackTemplate = "BuyPackTemplate";
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
    }
}
