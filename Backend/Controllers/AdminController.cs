using CsvHelper;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Http;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using MongoDB.Driver;

namespace Backend.Controllers
{
    public class AdminController : ControllerBase
    {
        private readonly IMongoCollection<Movie> _movies; // Updated type  
        private readonly IMongoCollection<Book> _books; // Updated type  
        private readonly IMongoCollection<TVSeries> _tvSeries; // Added field  

        public AdminController(IMongoDatabase database) // Constructor to initialize collections  
        {
            _movies = database.GetCollection<Movie>("Movies");
            _books = database.GetCollection<Book>("Books");
            _tvSeries = database.GetCollection<TVSeries>("TVSeries");
        }

        [HttpPost("upload-csv/{type}")]
        public async Task<IActionResult> UploadCsv(string type, IFormFile file, [FromHeader(Name = "x-admin-token")] string token)
        {
            if (token != "your-dev-token") return Unauthorized("Invalid token");
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            try
            {
                if (type == "books")
                {
                    var records = csv.GetRecords<Book>().ToList();
                    await _books.InsertManyAsync(records);
                }
                else if (type == "movies")
                {
                    var records = csv.GetRecords<Movie>().ToList();
                    await _movies.InsertManyAsync(records);
                }
                else if (type == "tvseries")
                {
                    var records = csv.GetRecords<TVSeries>().ToList();
                    await _tvSeries.InsertManyAsync(records);
                }
                else return BadRequest("Invalid type.");

                return Ok("CSV uploaded successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Upload failed: " + ex.Message);
            }
        }
    }
}
