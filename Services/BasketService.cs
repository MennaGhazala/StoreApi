using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstraction;
using Shared.BasketDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id)
        =>await basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDto> GetBasketAsync(string id)
        {
            var basket= await basketRepository.GetBasketAsync(id);

            return basket is null ? throw new BasketNotFoundException(id)
                : mapper.Map<BasketDto>(basket);
        }


        public async Task<BasketDto> UpdateBasketAsync(BasketDto basket)
        {
            var customerBasket = mapper.Map<CustomerBasket>(basket);
            var updateBasket = await basketRepository.UpdateBasketAsync(customerBasket);

            return updateBasket is null ? throw new Exception("Can't Update Basket Now !") : mapper.Map<BasketDto>(updateBasket);
            
        }
    }
}
