using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {
            var imageUrl = await FileHelper.UploadImage(song.Image);
            song.ImageUrl = imageUrl;

            var audioUrl = await FileHelper.UploadFile(song.AudioFile);
            song.AudioUrl = audioUrl;

            song.UploadedDate = DateTime.Now;
            await _dbContext.Songs1.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

       
        //pagging işlemi yapıldı 
        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;// default olarak 1 değeri gelir 
            int currentPageSize = pageSize ??= 2; // default olarak 2 değeri gelir 

            var songs = await (from song in _dbContext.Songs1
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl,
                               }).ToListAsync();
            return Ok(songs
                .Skip((currentPageNumber - 1) * currentPageSize)
                .Take(currentPageSize));

        }


        //IsFeatured == True listeleme 
        [HttpGet("[action]")]
        public async Task<IActionResult> FeatureSongs()
        {
            var songs = await (from song in _dbContext.Songs1
                               where song.IsFeatured == true
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl,
                               }).ToListAsync();

            return Ok(songs);

        }

        // Son Yüklenenler listesi
        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await (from song in _dbContext.Songs1
                               orderby song.UploadedDate descending
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl,
                               }).Take(1).ToListAsync(); // Take() -> kaç tanesini getireceğini belirler 

            return Ok(songs);

        } 

        //Aramaya göre listeleme
        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var songs = await (from song in _dbContext.Songs1
                               where song.Title.StartsWith(query)
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl,
                               }).Take(1).ToListAsync();

            return Ok(songs);

        }

    }
}
