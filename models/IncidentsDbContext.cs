using Microsoft.EntityFrameworkCore;

namespace incedentAPI_RimaBouazra.models
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions options) : base(options)//super(options)
        {
        }
        public virtual DbSet<Incident> Incidents { get; set; }//pour chaque classe une DBSet
        protected IncidentsDbContext()
        {
        }
    }
}
