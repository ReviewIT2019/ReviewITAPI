using ReviewIT.Backend.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReviewIT.Backend.Models.Repositories
{
    public interface IStageRepository : IDisposable
    {
        Task<int> CreateAsync(StageNoIdDTO stage);
        Task<StageDTO> FindAsync(int id);
        Task<IReadOnlyCollection<StageDTO>> ReadAsync();
        Task<bool> UpdateAsync(StageDTO stage);
        Task<bool> DeleteAsync(int id);
    }
}
