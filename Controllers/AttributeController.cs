using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly AttributeRepository _attributeRepository;
        public AttributeController(AttributeRepository attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }
        // GET: api/<AttributeController>
        [HttpGet]
        public IActionResult Get()
        {
          
            var attributes = _attributeRepository.GetAll();
            return Ok(attributes);
        }

        // GET api/<AttributeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AttributeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AttributeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AttributeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
