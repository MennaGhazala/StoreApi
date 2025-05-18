using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IOrderService
    {
        Task<OrderResult> GetOrderByIdAsync(Guid id);
        Task<IEnumerable< OrderResult>> GetOrderByEmailAsync(string email);

        Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest , string buyEmail);

        Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodAsync();

    }
}
