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
            Claim? userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (roleClaim != null && userId != null && roleClaim.Value == "customer")
            {
                return int.Parse(userId.Value);
            }

            return null;
        }

        public bool IsUserStaff()
        {
            Claim? roleClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            Claim? userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (roleClaim != null && roleClaim.Value == "stuff")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
