using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantAttributeController : ControllerBase
    {
        private
   readonly VariantAttributeRepository _repository;


        public VariantAttributeController(VariantAttributeRepository repository)
        {
            _repository = repository;
        }
        // GET: api/<VariantAttributeController>
        [HttpGet]
        public IActionResult Get()
        {

            var list = _repository.GetAll();


            return Ok(list);

            
        }

        // GET api/<VariantAttributeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VariantAttributeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<VariantAttributeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VariantAttributeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost("updatelist")]
        public IActionResult UpdateFromList(List<VariantAttributeView> list)
        {
            var result = _repository.UpdateFromList(list);
            return Ok(result);
        }
    }
}
