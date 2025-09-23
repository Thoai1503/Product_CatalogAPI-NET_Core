using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeController : ControllerBase
    {
        private 
            readonly ProductAttributeRepository _repository;

        public ProductAttributeController (ProductAttributeRepository repository)
        {
            _repository = repository;
        }
       
        // GET: api/<ProductAttributeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProductAttributeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductAttributeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductAttributeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,ProductAttributeView entity)
        {
            var en = _repository.FindById(id);
            en.ValueInt = entity.value_int;
            en.ValueText = entity.value_text;
            en.ValueDecimal = entity.value_decimal;
            var result = _repository.Update(en);

                return Ok(result);
        }

        // DELETE api/<ProductAttributeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
