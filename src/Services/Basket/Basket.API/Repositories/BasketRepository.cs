using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task DeleteBasket(string UserName)
        {
            await _redisCache.RemoveAsync(UserName);
        }

        public async Task<ShoppingCart> GetBasket(string username)
        {
            var basket=await _redisCache.GetStringAsync(username);
            if(String.IsNullOrEmpty(basket))
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            await _redisCache.SetStringAsync(shoppingCart.UserName,JsonConvert.SerializeObject(shoppingCart));
            return await GetBasket(shoppingCart.UserName);
        }
    }
}