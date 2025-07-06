using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMongoCollection<Book> _books;
        private readonly IMongoCollection<Movie> _movies;
        private readonly IMongoCollection<TVSeries> _tvSeries;

        public AdminController(IOptions<MongoDBSettings> settings)
        {
            var db = new MongoClient(settings.Value.ConnectionString)
                         .GetDatabase(settings.Value.DatabaseName);
            _books = db.GetCollection<Book>("Books");
            _movies = db.GetCollection<Movie>("Movies");
            _tvSeries = db.GetCollection<TVSeries>("TVSeries");
        }

        [HttpPost("books")]
        public async Task<IActionResult> UploadBook([FromBody] Book book)
        {
            await _books.InsertOneAsync(book);
            return StatusCode(201, "Book uploaded.");
        }

        [HttpPost("movies")]
        public async Task<IActionResult> UploadMovie([FromBody] Movie movie)
        {
            await _movies.InsertOneAsync(movie);
            return StatusCode(201, "Movie uploaded.");
        }

        [HttpPost("tvseries")]
        public async Task<IActionResult> UploadSeries([FromBody] TVSeries series)
        {
            await _tvSeries.InsertOneAsync(series);
            return StatusCode(201, "TV Series uploaded.");
        }
    }
}
