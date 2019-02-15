using Microsoft.EntityFrameworkCore;
using ReviewIT.Backend.Entities.Entities;

namespace ReviewIT.Backend.Entities.Contexts
{
    public class Context : DbContext, IContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Study> Studies { get; set; }
    }
}
