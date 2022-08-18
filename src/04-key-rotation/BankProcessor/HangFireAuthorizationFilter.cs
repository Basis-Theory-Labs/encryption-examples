using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace BankProcessor;

public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize([NotNull] DashboardContext context) => true;
}
