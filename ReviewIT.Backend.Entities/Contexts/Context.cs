using Microsoft.EntityFrameworkCore;
using ReviewIT.Backend.Entities.Entities;
using ReviewIT.Backend.Entities.Entitities;

namespace ReviewIT.Backend.Entities.Contexts
{
    public class Context : DbContext, IContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Study> Studies { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
    }
}
