using CatalogServiceAPI_Electric_Store.Models.Entities;
using CatalogServiceAPI_Electric_Store.Models.ModelView;
using CatalogServiceAPI_Electric_Store.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CatalogServiceAPI_Electric_Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult Get()
        {
            var category = _categoryRepository.GetAll();
            return Ok(category);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoryController>
        [HttpPost]
        public IActionResult Post(CategoryView cate)
        {
            var result = _categoryRepository.CreateAndReturn(cate);
            if (result ==null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        // PUT api/<CategoryController>/5
        [HttpPost("{id}")]
        public IActionResult Put(int id, CategoryView cate)
        {
            var en = _categoryRepository.GetEntityById(id); // EF đang track en
            if (en != null)
            {
                // update trực tiếp
                en.Name = en.Name;
                en.Slug = cate.slug;
                en.ParentId = cate.parent_id;
                en.Path = cate.path;
                en.Level = cate.level;
                en.CreatedAt = DateTime.Now;

                var result = _categoryRepository.Update(en);
                return Ok(result);
            }
            return NotFound();
        }


        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           var re = _categoryRepository.Delete(id);
            if (!re)
            {
                return NotFound();
            }
            return Ok(re);
        }
        [HttpGet("slug/{slug}")]
        public ActionResult<string> GetBySlug(string slug)
        {
            return Ok($"Category with slug: {slug}");
        }
    }
}
