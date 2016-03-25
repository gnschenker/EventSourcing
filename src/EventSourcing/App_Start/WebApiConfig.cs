using System.Web.Http;

namespace EventSourcing
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
       }
    }
}
