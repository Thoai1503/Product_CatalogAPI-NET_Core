using Azure.Core;
using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private
          readonly ProductImageRepository _repository;


        public ProductImageController(ProductImageRepository repository)
        {
            _repository = repository;
        }
        // GET: api/<ProductImageController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProductImageController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductImageController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductImageController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductImageController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _repository.Delete(id);
            return Ok(result);
        }
        [HttpPost("variant/{variantId}")]
        public async Task<IActionResult> CreateByVariousId(List< IFormFile> images, [FromForm] int product_id,
    [FromForm] int variant_id) 
        {

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var item in images)
            {
                Console.WriteLine("File name: "+item.FileName);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
                var en = new ProductImageView
                {
                    product_id = product_id,
                    variant_id = variant_id,
                    url=uniqueFileName,

                };
                var result= _repository.Create(en);
                if(!result) return NotFound();
            }

            return Ok(true);
        }
        [HttpGet("variant/{variantId}")]
        public IActionResult GetByVariantId(int variantId) { 
              var list = _repository.GetByVariantId(variantId);
            return Ok(list);    
        }
    }
}
