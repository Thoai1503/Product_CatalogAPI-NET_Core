using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeValueController : ControllerBase
    {

        private readonly AttributeValueRepository
            _attributeValueRepository;
        public AttributeValueController(AttributeValueRepository attributeValueRepository)
            {
            _attributeValueRepository = attributeValueRepository;
        }
        // GET: api/<AttributeValueController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AttributeValueController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AttributeValueController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AttributeValueController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AttributeValueController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("Attribute/{attributeId}")]
        public IActionResult GetByAttributeId(int attributeId)
        {
            var attributeValues = _attributeValueRepository.GetByAttributeId(attributeId);
          
            return Ok(attributeValues);
        }
    }
}
