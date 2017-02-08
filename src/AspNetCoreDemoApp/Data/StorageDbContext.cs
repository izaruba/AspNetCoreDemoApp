using Microsoft.EntityFrameworkCore;

namespace AspNetCoreDemoApp
{
    public class StorageDbContext : ApplicationDbContext
    {
        public StorageDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}