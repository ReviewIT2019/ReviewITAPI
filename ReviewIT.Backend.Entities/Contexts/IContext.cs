using Microsoft.EntityFrameworkCore;
using ReviewIT.Backend.Entities.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReviewIT.Backend.Entities.Contexts
{
    public interface IContext : IDisposable
    {
        DbSet<Study> Studies { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
