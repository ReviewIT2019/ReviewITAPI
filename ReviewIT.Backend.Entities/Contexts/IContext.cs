using Microsoft.EntityFrameworkCore;
using ReviewIT.Backend.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewIT.Backend.Entities.Contexts
{
    public interface IContext : IDisposable
    {
        DbSet<Study> Studies { get; set; }
    }
}
