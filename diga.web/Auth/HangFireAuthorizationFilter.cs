using System.Web;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace diga.web.Auth
{
    public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            //Can use this for NetCore
            return context.GetHttpContext().User.Identity.IsAuthenticated; 
        }
    }
}
