using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using Basket.API.Entities;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }
        
        

        [HttpGet("{username}",Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket=await _repository.GetBasket(username);
            return Ok(basket ?? new ShoppingCart(username));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody]ShoppingCart shoppingCart)
        {   
            return Ok(await _repository.UpdateBasket(shoppingCart));
        }

        [HttpDelete("{username}",Name ="DeleteBasket")]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> DeleteBasket(string UserName)
        {
            await _repository.DeleteBasket(UserName);
            return Ok();
        }
    }
}