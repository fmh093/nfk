using Microsoft.AspNetCore.Mvc;
using NFKApplication.Database;
using NFKApplication.Database.Models;
using NFKApplication.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NFKApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get(int id)
        {
            var basketFound = _basketRepository.TryGetBasket(id, out var basket);
            if (!basketFound)
            {
                return NotFound();
            }

            return Ok(basket);
        }

        // GET api/<ValuesController>/5
        //[HttpGet("{sku}")]
        //public string Get(string sku)
        //{
        //    return "value";
        //}

        [HttpPost("AddToBasket")]
        public IActionResult AddToBasket([FromBody] AddToBasketRequest request)
        {
            var basket = _basketRepository.AddToBasket(request.BasketId, request.Sku, request.Amount);
            if (basket == null)
            {
                return NotFound();
            }

            return value == null ? default(T) : JsonSerializer.Deserialize<BasketDto>(value);

            return Ok(basket);
        }

        public class AddToBasketRequest
        {
            public int BasketId { get; set; }
            public string Sku { get; set; }
            public int Amount { get; set; }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
