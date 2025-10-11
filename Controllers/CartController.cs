using Microsoft.AspNetCore.Mvc;
using CatalogServiceAPI_Electric_Store.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class CartController : ControllerBase
    {
        private CartRepository _cartRepository;

        public CartController (CartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        // GET: api/<CartController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CartController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CartController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("user/{userId}")]
        public IActionResult FindByUserId(int userId)
        {

            var list = _cartRepository.FindByUserId(userId);
            return Ok(list);

        }
    }
}
