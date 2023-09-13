using BikeStore.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BikeStore.WebApi.Infra
{
    public class ResourceAuthorizationHandler : AuthorizationHandler<CustomersResourceRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       CustomersResourceRequirement requirement,
                                                       int customerId)
        {
            Claim? roleClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            Claim? userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (roleClaim != null && userId != null)
            {
                if (roleClaim.Value == RoleConstants.StaffRoleName || (roleClaim.Value == RoleConstants.CustomerRoleName && int.Parse(userId.Value) == customerId))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }

            return Task.CompletedTask;
        }
    }

    public class CustomersResourceRequirement : IAuthorizationRequirement { }
}
