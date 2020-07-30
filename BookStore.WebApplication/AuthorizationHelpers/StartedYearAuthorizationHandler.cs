using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace BookStore.WebApplication.AuthorizationHelpers
{
    public class StartedYearAuthorizationHandler : AuthorizationHandler<StartedYearRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StartedYearRequirement requirement)
        {
            var claimType = "CompanyStarted";
            if (!context.User.HasClaim(c => c.Type == claimType))
            {
                return Task.CompletedTask;
            }

            var userStartedYear = DateTimeOffset.Parse(context.User.FindFirst(c => c.Type == claimType).Value).Year;

            if(requirement.StartedYear >= userStartedYear)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
