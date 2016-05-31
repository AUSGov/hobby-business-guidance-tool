namespace Sb.Entities
{
    public class Constants
    {
        public class SubmitType
        {
            public static string Previous = "previous";
            public static string Next = "next";
            public static string Results = "results";
        }

        public class Environment
        {
            public static string Prod = "prod";
            public static string Test = "test";
            public static string Dev = "dev";
            public static string Local = "local";
        }

        public class CookieName
        {
            public static string Answers = "answers";
        }

        public class AppSettings
        {
            public static string GoogleTagManagerId = "Sb.Analytics.GoogleTagManagerId";
            public static string SitecoreFxmBeaconUrl = "Sb.Analytics.SitecoreFxmBeaconUrl";
            public static string EnvironmentType = "Sb.EnvironmentType";
            public static string RobotsFile = "Sb.RobotsFile";
            public static string SiteTakeoverHtml = "Sb.SiteTakeoverHtml";
            public static string FeedbackUrl = "Sb.FeedbackUrl";
            public static string AdminPagesEnabled = "Sb.AdminPagesEnabled";
        }
    }
}