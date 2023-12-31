using BikeStore.Application.Services;
using BikeStore.Domain.Constants;
using BikeStore.Domain.Entities.Sales;
using BikeStore.Infrastructure.EntityFramework.Identity;
using BikeStore.WebApi.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserClaimsService _userClaimsService;

        public OrderController(OrderService orderService, IAuthorizationService authorizationService,
            UserClaimsService userClaimsService)
        {
            _orderService = orderService;
            _authorizationService = authorizationService;
            _userClaimsService = userClaimsService;
        }

        [HttpGet("{pageSize},{pageNumber}, {customerId}")]
        public async Task<List<Order>> Get(int pageSize = 1, int pageNumber = 1, int? customerId = null)
        {
            
            if(!User.IsInRole(RoleConstants.StaffRoleName))
            {
                if(customerId != null)
                {
                    await _authorizationService.AuthorizeAsync(User, customerId, "ViewCreateEditPolicyForCustomer");
                }
                else
                {
                    int? currentCustomerId = _userClaimsService.GetCurrentCustomerIdIfExists();

                    await _authorizationService.AuthorizeAsync(User, currentCustomerId, "ViewCreateEditPolicyForCustomer");

                    return await _orderService.GetWithPaginationAsync(pageSize, pageNumber, currentCustomerId);
                }
            }

            return await _orderService.GetWithPaginationAsync(pageSize, pageNumber, customerId);
        }

        [HttpGet("{orderId}")]
        public async Task<Order?> GetById(int orderId)
        {
            Order? order = await _orderService.GetAsyncById(orderId);

            if(order != null && order.CustomerId.HasValue)
            {
                await _authorizationService.AuthorizeAsync(User, order.CustomerId, "ViewCreateEditPolicyForCustomer");
            }

            return order;
        }

        [HttpPost]
        public async Task<Order> Create(Order order)
        {
            if (order.CustomerId.HasValue)
            {
                await _authorizationService.AuthorizeAsync(User, order.CustomerId, "ViewCreateEditPolicyForCustomer");
            }

            return await _orderService.CreateAsync(order);
        }

        [HttpPut("Cancel")]
        public async Task<int> Cancel(int orderId)
        {
            Order? order = await _orderService.GetAsyncById(orderId);

            if (order != null && order.CustomerId.HasValue)
            {
                await _authorizationService.AuthorizeAsync(User, order.CustomerId, "ViewCreateEditPolicyForCustomer");
            }

            return await _orderService.CancelAsync(orderId);
        }
    }
}
