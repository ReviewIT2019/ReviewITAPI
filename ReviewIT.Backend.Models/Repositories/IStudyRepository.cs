using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReviewIT.Backend.Common.DTOs;

namespace ReviewIT.Backend.Models.Repositories
{
    public interface IStudyRepository
    {
        Task<IReadOnlyCollection<StudyDTO>> ReadAsync();
    }
}
