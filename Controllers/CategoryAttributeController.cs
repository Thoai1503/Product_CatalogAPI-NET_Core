using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAttributeController : ControllerBase
    {
        private readonly CategoryAttributeRepository _categoryAttributeRepository;

    
        public  CategoryAttributeController(CategoryAttributeRepository categoryAttributeRepository)
        {
          _categoryAttributeRepository = categoryAttributeRepository;
        }

        // GET: api/<CategoryAttributeController>
        [HttpGet]
        public IActionResult Get()
        {
           
            var categoryAttributes = _categoryAttributeRepository.GetAll();
            return Ok(categoryAttributes);
        }

        // GET api/<CategoryAttributeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoryAttributeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CategoryAttributeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryAttributeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("category/{categoryId}")]
        public IActionResult GetByCategoryId(int categoryId)
        {
            var categoryAttributes = _categoryAttributeRepository.GetByCategoryId(categoryId);
            return Ok(categoryAttributes);
        }
    }
}
