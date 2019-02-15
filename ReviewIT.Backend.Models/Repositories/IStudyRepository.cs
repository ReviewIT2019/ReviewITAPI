using ReviewIT.Backend.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReviewIT.Backend.Models.Repositories
{
    public interface IStudyRepository : IDisposable
    {
        Task<int> CreateAsync(StudyNoIdDTO study);
        Task<StudyDTO> FindAsync(int id);
        Task<IReadOnlyCollection<StudyDTO>> ReadAsync();
        Task<bool> UpdateAsync(StudyDTO study);
        Task<bool> DeleteAsync(int id);
    }
}
