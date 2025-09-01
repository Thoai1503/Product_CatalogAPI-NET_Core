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
        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult Get()
        {
            var category = CategoryRepository.Instance.GetAll();
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
            var result = CategoryRepository.Instance.CreateAndReturn(cate);
            if (result ==null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, CategoryView cate)
        {
            var en = CategoryRepository.Instance.GetEntityById(id); // EF đang track en
            if (en != null)
            {
                // update trực tiếp
                en.Name = cate.name;
                en.Slug = cate.slug;
                en.ParentId = cate.parent_id;
                en.Path = cate.path;
                en.Level = cate.level;
                en.CreatedAt = DateTime.Now;

                var result = CategoryRepository.Instance.Update(en);
                return Ok(result);
            }
            return NotFound();
        }


        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           var re = CategoryRepository.Instance.Delete(id);
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
