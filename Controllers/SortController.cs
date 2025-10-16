using Algorithm.Core.Models;
using Algorithm.Core.Services.Algorithms;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Algorithm_Animation_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortController : ControllerBase
    {
        // GET: api/<SortController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SortController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SortController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SortController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SortController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost("{algorithm}")]
        public IActionResult Sort(string algorithm, [FromBody] int[] array)
        {
            List<SortStep> steps = algorithm.ToLower() switch
            {
                "bubble" => BubbleSort.Sort(array),
                _ => throw new Exception("Unsupported algorithm")
            };

            return Ok(steps);
        }
    }
}
