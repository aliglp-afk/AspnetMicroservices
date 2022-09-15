using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart>GetBasket(string username);
        Task<ShoppingCart>UpdateBasket(ShoppingCart shoppingCart);
        Task DeleteBasket(string UserName);

        
    }
}