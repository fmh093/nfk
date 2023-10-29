using Microsoft.AspNetCore.Mvc;
using NFKApplication.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NFKApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("GetBasket")]
        public IActionResult GetBasket()
        {
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
            basket = _basketService.AddToBasket(basket.Id, request.Sku, request.Amount);

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

            _basketService.UpdateBasket(basket);

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
