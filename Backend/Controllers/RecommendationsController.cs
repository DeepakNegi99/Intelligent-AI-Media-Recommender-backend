using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("recommendations")]
    public class RecommendationsController : ControllerBase
    {
        private readonly RecommendationService _recommendationService;

        public RecommendationsController(RecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpPost]
        public async Task<IActionResult> GetRecommendations([FromBody] UserPreferences preferences)
        {
            var results = await _recommendationService.GetRecommendationsAsync(preferences);
            return Ok(results);
        }
    }
}
