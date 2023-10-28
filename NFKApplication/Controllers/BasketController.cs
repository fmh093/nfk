using Microsoft.AspNetCore.Authorization;
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
            var basket = _basketService.GetBasket(HttpContext);
            if (basket == null)
            {
                return NotFound();
            }
            basket = _basketRepository.AddToBasket(basket.Id, request.Sku, request.Amount);

            return Ok(basket);
        }

        [HttpPost("UpdateBasketInformation")]
        public IActionResult UpdateBasketInformation([FromBody] UpdateBasketInformationRequest updateBasketInformationRequest)
        {
            var basket = _basketService.GetBasket(HttpContext);
            if (basket == null)
            {
                return BadRequest();
            }
            basket.FirstName = updateBasketInformationRequest.FirstName;
            basket.LastName = updateBasketInformationRequest.LastName;
            basket.Address = updateBasketInformationRequest.Address;

            _basketRepository.UpdateBasket(basket);

            return NoContent();
        }


        public class AddToBasketRequest
        {
            public string Sku { get; set; } = string.Empty;
            public int Amount { get; set; } = 1;
        }
        public class UpdateBasketInformationRequest
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
        }
    }
}
