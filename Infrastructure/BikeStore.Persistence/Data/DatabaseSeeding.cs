using BikeStore.Domain.Constants;
using BikeStore.Infrastructure.EntityFramework.Identity;
using BikeStore.WebApi.Models;
using Microsoft.AspNetCore.Identity;

namespace BikeStore.Infrastructure.EntityFramework.Data
{
    public class DatabaseSeeding
    {
        private readonly RoleManager<Role> _roleManager;

        private readonly BikeStoreDbContext _bikeStoreDbContext;

        public DatabaseSeeding(RoleManager<Role> roleManager, BikeStoreDbContext bikeStoreDbContext)
        {
            _roleManager = roleManager;
            _bikeStoreDbContext = bikeStoreDbContext;
        }
        public async Task Seed()
        {
            await _bikeStoreDbContext.Database.EnsureCreatedAsync();

            await SeedRoles();
        }

        private async Task SeedRoles()
        {
            if (!_roleManager.Roles.Any())
            {
                Role customerRole = new()
                {
                    Id = "ebd9a85c-ee6a-4cff-81d1-1619900823b7",
                    Name = RoleConstants.CustomerRoleName
                };
                Role staffRole = new()
                {
                    Id = "bf04bd16-9b85-4889-9d72-b299cb532dc8",
                    Name = RoleConstants.StaffRoleName
                };

                await _roleManager.CreateAsync(customerRole);
                await _roleManager.CreateAsync(staffRole);
            }
        }
    }
}
