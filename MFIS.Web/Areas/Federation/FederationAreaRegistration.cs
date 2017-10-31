using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation
{
    public class FederationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Federation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Federation_default",
                "Federation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
