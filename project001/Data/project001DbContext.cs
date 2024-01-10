using Microsoft.EntityFrameworkCore;
using project001.Models.Domain;

namespace project001.Data
{
    public class project001DbContext : DbContext
    {
        public project001DbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
