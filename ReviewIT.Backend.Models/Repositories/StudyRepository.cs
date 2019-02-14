using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ReviewIT.Backend.Common.DTOs;
using ReviewIT.Backend.Entities.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ReviewIT.Backend.Models.Repositories
{
    public class StudyRepository : IStudyRepository
    {
        private readonly IContext _context;

        public StudyRepository(IContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyCollection<StudyDTO>> ReadAsync()
        {
            var studies = from study in _context.Studies
                          select new StudyDTO
                          {
                              Id = study.Id,
                              Title = study.Title,
                              Description = study.Description
                          };

            return await studies.ToListAsync();
        }
    }
}
