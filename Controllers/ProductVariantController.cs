using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private
           readonly ProductVariantRepository _repository;


        public ProductVariantController(ProductVariantRepository repository)
        {
            _repository = repository;
        }
        // GET: api/<ProductVariantController>
        [HttpGet]
        public IActionResult Get([FromQuery] FilterState st)
            {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(st.attributes));
            Console.WriteLine("Query:" + st);
           var list = _repository.GetPaginationData(st);
            return Ok(list);
        }

        // GET api/<ProductVariantController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _repository.FindById(id);
            return Ok(result);
        }

        // POST api/<ProductVariantController>
        [HttpPost]
        public IActionResult Post([FromBody] ProductVariantView en)
        {
            var re = _repository.Create(en);
            return Ok(re);
        }

        // PUT api/<ProductVariantController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
                
        }
        [HttpPost("Update")]
        public IActionResult UpdateVariant(ProductVariantView en)
        {
            var results = _repository.Update(en);
            if(!results) return NotFound();
            return Ok(results);
        }

        // DELETE api/<ProductVariantController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            
            var re = (_repository.Delete(id));
            return Ok(re);

        }

        [HttpGet("product/{productId}")]
        public IActionResult FindByProductId(int productId)
        {

            var list = _repository.FindByProductId(productId);


            return Ok(list);
        }
    }
}
