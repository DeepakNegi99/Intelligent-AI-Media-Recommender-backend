using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("user/preferences")]
    public class UserPreferencesController : ControllerBase
    {
        private readonly UserPreferenceService _preferenceService;

        public UserPreferencesController(UserPreferenceService preferenceService)
        {
            _preferenceService = preferenceService;
        }

        [HttpPost]
        public async Task<IActionResult> SavePreferences([FromBody] UserPreferences preferences)
        {
            if (preferences == null || preferences.FavoriteGenres.Count == 0)
                return BadRequest("Invalid preference data.");

            await _preferenceService.CreateAsync(preferences);
            return StatusCode(201, "Preferences saved successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var prefs = await _preferenceService.GetAllAsync();
            return Ok(prefs);
        }
    }
}
