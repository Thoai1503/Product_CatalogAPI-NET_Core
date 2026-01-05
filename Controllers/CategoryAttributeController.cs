using CatalogServiceAPI_Electric_Store.Models.ModelView;
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


        public CategoryAttributeController(CategoryAttributeRepository categoryAttributeRepository)
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
        public IActionResult Post([FromBody] CategoryAttributeView en)
        {
            var result = _categoryAttributeRepository.Create(en);
            if (!result) BadRequest(result);
            return Ok(result);
        }

        // PUT api/<CategoryAttributeController>/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CategoryAttributeUpdateState en)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _categoryAttributeRepository.FindById(id);
            if (entity == null)
                return NotFound(new { message = "Category attribute not found" });

            // Cập nhật dữ liệu
            entity.category_id = en.category_id;
            entity.attribute_id = en.attribute_id;
            entity.is_filterable = en.is_filterable;
            entity.is_variant_level = en.is_variant_level;
            entity.is_required = en.is_required;

            var result = _categoryAttributeRepository.Update(entity);
            if (!result)
                return BadRequest(new { success = false, message = "Failed to update category attribute" });

            return Ok( entity );
        }

        // DELETE api/<CategoryAttributeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var re = _categoryAttributeRepository.Delete(id);
            return Ok(re);
        }
        [HttpGet("category/{category}")]
        public IActionResult GetByCategory(string category)
        {
            var categoryAttributes = _categoryAttributeRepository.GetByCategory(category);
            return Ok(categoryAttributes);
        }
        [HttpPost("category/{categoryId}")]
        public IActionResult CreateMultipleAttr(int categoryId, int[] ints)
        {
            Console.WriteLine("Array of int: " + ints);
            for (int i = 0; i < ints.Length; i++)
            {
                Console.WriteLine(ints[i]);
                var re = _categoryAttributeRepository.Create(new CategoryAttributeView { attribute_id = ints[i], category_id = categoryId });
                if (!re)
                {
                    // Return a valid anonymous object using C# syntax
                    return Ok(new { success = false, id = ints[i] });
                }
            }
            return Ok(true);
        }
        //[HttpGet("category/{categoryId}")]
        //public IActionResult GetAttributesByCategoryId(int categoryId)
        //{
        //    var categoryAttributes = _categoryAttributeRepository.GetByCategoryId(categoryId);
        //    return Ok(categoryAttributes);
        //}

    }
}
