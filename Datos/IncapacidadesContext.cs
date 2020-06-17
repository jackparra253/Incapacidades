using Microsoft.EntityFrameworkCore;

namespace Datos
{
    public class IncapacidadesContext : DbContext
    {
        public IncapacidadesContext(DbContextOptions<IncapacidadesContext> options) : base(options)
        {

        }

        public IncapacidadesContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Test> Tests { get; set; }

    }

    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
