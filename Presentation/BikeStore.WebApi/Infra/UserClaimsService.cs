using BikeStore.Domain.Constants;
using System.Security.Claims;

namespace BikeStore.WebApi.Infra
{
    public class UserClaimsService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public UserClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetCurrentCustomerIdIfExists()
        {
            Claim? roleClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            Claim? customerId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (roleClaim != null && customerId != null && roleClaim.Value == RoleConstants.CustomerRoleName)
            {
                return int.Parse(customerId.Value);
            }

            return null;
        }
    }
}
