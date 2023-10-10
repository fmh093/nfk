using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFKApplication.Database;
using NFKApplication.Database.Models;
using NFKApplication.Models;
using NFKApplication.Services;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NFKApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IBasketRepository _basketRepository;
        private readonly IBasketService _basketService;
        public BasketController(IBasketRepository basketRepository, IBasketService basketService)
        {
            _basketRepository = basketRepository;
            _basketService = basketService;
        }

        [HttpGet("GetBasket")]
        public IActionResult GetBasket()
        {
            // todo implement add to basket on product details page
            var basket = _basketService.GetBasket(HttpContext);
            return Ok(basket);
        }

        [HttpPost("AddToBasket")]
        public IActionResult AddToBasket([FromBody] AddToBasketRequest request)
        {
            // todo find a smarter way to ensure that basket exists and create it if it doesn't
            var basket = _basketRepository.AddToBasket(request.BasketId, request.Sku, request.Amount);
            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }

        [HttpPost("SaveBasket")]
        public IActionResult SaveBasket([FromBody] Basket updatedBasket)
        {
            if (updatedBasket == null)
            {
                return BadRequest();
            }
            _basketRepository.SaveBasket(updatedBasket);

            return NoContent();
        }

        public class AddToBasketRequest
        {
            public int BasketId { get; set; }
            public string Sku { get; set; } = string.Empty;
            public int Amount { get; set; } = 1;
        }
    }
}
