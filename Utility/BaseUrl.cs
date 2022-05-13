using System.Configuration;

namespace EFreshStore.Utility
{
    public static class BaseUrl
    {
        //public static string baseUrl = "http://dotnet.nerdcastlebd.com/EFreshApiTest/";
        //public static string url = "http://dotnet.nerdcastlebd.com/EFreshApiTest/api/";

        public static string baseUrl =ConfigurationManager.AppSettings["baseUrl"].ToString();
        public static string homeUrl =ConfigurationManager.AppSettings["HomeUrl"].ToString();
        public static string url = ConfigurationManager.AppSettings["url"].ToString();
        public static string subDirectory = ConfigurationManager.AppSettings["SubDirectory"].ToString();
        //public static string url = "http://localhost:50644/api/";
       // public static string baseUrl = "http://localhost:50644/api/";
    }
}