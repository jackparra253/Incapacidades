using Datos;
using Microsoft.EntityFrameworkCore;

namespace Test
{
    public abstract class TestBase
    {
        private bool _useSqlite;

        public IncapacidadesContext GetDbContext()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            if (_useSqlite)
            {
                builder.UseSqlite("DataSource=:memory:", x => { });
            }
            
            
            var dbContext = new IncapacidadesContext(builder.Options);
            if (_useSqlite)
            {
                 dbContext.Database.OpenConnection();
            }

            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        public void UseSqlite()
        {
            _useSqlite = true;
        }
    }
}
