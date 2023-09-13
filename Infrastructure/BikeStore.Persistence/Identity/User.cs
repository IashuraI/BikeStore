using BikeStore.Domain.Entities.Sales;
using Microsoft.AspNetCore.Identity;

namespace BikeStore.Infrastructure.EntityFramework.Identity
{
    public class User : IdentityUser<string>
    {
        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}
