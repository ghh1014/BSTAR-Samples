using System.Web.Http;

namespace AxisMgntServer
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultAreaApi",
                routeTemplate: "api/{version}/{controller}/{action}"
           );
        }
    }
}
