using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Hangfire.Runner
{
    public class DashboardFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}