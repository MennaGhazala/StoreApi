using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.OrderEntities;
using Domain.Exceptions;
using Services.Abstraction;
using Services.Specifications;
using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService (IUnitOfWork  unitOfWork , IMapper mapper, IBasketRepository basketRepository): IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string buyEmail)
        {
            //address
            var address = mapper.Map<Address>(orderRequest.ShippingAddress);
            //basket
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId);

            if (basket is null)
                throw new BasketNotFoundException(orderRequest.BasketId);
           var orderItems = new List<OrderItem>();

            foreach(var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id);
                if (product is null)
                    throw new ProductNotFoundException(item.Id);
                var productInOrderItem = new ProductInOrderItem(product.Id, product.Name, product.PictureUrl);

                var orderItem = new OrderItem(productInOrderItem, item.Quantity, product.Price);

                orderItems.Add(orderItem);
            }

            //delivery method
            var deliveryMehod= await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequest.DeliveryMethodId);
            if(deliveryMehod is null)
                throw new DeliveryMethodNotFoundException (orderRequest.DeliveryMethodId);
            //subtotal
            var subtotal = orderItems.Sum(x => x.Quantity*x.Price);

            var order =new Order(buyEmail,address,orderItems,deliveryMehod,subtotal);
    
            await unitOfWork.GetRepository<Order,Guid>().AddAsync(order);
            await unitOfWork.SaveChangesAsync();
            return mapper.Map<OrderResult>(order);

        }

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodAsync()
        {
          var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();

            return mapper.Map<IEnumerable<DeliveryMethodResult>>(deliveryMethod);
        }

        public  async Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email)
        {
            var ordersSpecs = new OrderWithIncludeSpecification(email);
            var order = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(ordersSpecs);

            

            return mapper.Map<IEnumerable<OrderResult>>(order);


        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var orderSpecs = new OrderWithIncludeSpecification(id);
            var order =await unitOfWork .GetRepository<Order, Guid>().GetAsync(orderSpecs);

            if (order is null)
                throw new OrderNotFoundException(id);

            return mapper.Map<OrderResult>(order);
        }
    }
}
