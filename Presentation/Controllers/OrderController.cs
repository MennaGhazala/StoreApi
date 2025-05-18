﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class OrderController(IServiceManager serviceManager) : ApiController
    {
        [HttpPost]

        public async Task<ActionResult<OrderResult>> Create(OrderRequest orderRequest)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var order = await serviceManager.OrderService.CreateOrderAsync(orderRequest, email);

            return Ok(order);

        }

        [HttpGet]
        public async Task<ActionResult<OrderResult>> GetOrderById(Guid id)
         => Ok(await serviceManager.OrderService.GetOrderByIdAsync(id));
        [HttpGet]

        public async Task<ActionResult<IEnumerable<OrderResult>>> GetOrderByEmail()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var order = await serviceManager.OrderService.GetOrderByEmailAsync(email);
                return Ok(order);
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<DeliveryMethodResult>>> GetDeliveryMethods()
         => Ok(await serviceManager.OrderService.GetDeliveryMethodAsync());

    }
}
