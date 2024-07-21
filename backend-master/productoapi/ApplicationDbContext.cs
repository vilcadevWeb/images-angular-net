using Microsoft.EntityFrameworkCore;
using productoapi.Models;

namespace productoapi
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Producto> Productos { get; set; }
    }
}
