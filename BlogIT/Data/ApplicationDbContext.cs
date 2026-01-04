using BlogIT.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogIT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        DbSet<Category> Categories { get; set; }
    }
}
