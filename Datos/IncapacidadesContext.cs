using Microsoft.EntityFrameworkCore;

namespace Datos
{
    public class IncapacidadesContext : DbContext
    {
        public IncapacidadesContext(DbContextOptions<IncapacidadesContext> options): base (options)
        {
            
        }

        
    }
}
