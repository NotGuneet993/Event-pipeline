using Microsoft.AspNetCore.Mvc;
using Consumer.Services;

namespace Consumer.Controllers
{
    [ApiController]
    [Route("/api")]
    public class SearchController : Controller
    {
        private readonly ElasticsearchService _esService;

        public SearchController(ElasticsearchService esService)
        {
            _esService = esService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("Query parameter 'q' is required");
            }

            var results = await _esService.SearchAsync(q);  
            return Ok(results);
        }
    }
}
