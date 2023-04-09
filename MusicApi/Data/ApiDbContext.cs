using Microsoft.EntityFrameworkCore;
using MusicApi.Models;

namespace MusicApi.Data
{
    public class ApiDbContext :DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext>options) :base(options)
        {

        }
        public DbSet<Song> Songs1 { get; set; }
        public DbSet<Artist> Artists1 { get; set; }
        public DbSet<Album> Albums1 { get; set; }

    }
}
