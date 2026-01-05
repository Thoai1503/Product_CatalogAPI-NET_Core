using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryBrandController : ControllerBase
    {
        private readonly CategoryBrandRepository _categoryBrandRepository;

        public CategoryBrandController (CategoryBrandRepository categoryBrandRepository)
        {
            _categoryBrandRepository = categoryBrandRepository;
        }
        // GET: api/<CategoryBrandController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CategoryBrandController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoryBrandController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CategoryBrandController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryBrandController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("category/{category}")]
        public IActionResult GetByCategory (string category)
        {
            var list = _categoryBrandRepository.GetByCategory(category);

            return Ok(list);
        }
    }
}
