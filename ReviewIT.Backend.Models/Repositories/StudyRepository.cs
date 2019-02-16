using Microsoft.EntityFrameworkCore;
using ReviewIT.Backend.Common.DTOs;
using ReviewIT.Backend.Entities.Contexts;
using ReviewIT.Backend.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewIT.Backend.Models.Repositories
{
    public class StudyRepository : IStudyRepository
    {
        private readonly IContext _context;

        public StudyRepository(IContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(StudyNoIdDTO study)
        {
            var entity = new Study
            {
                Title = study.Title,
                Description = study.Description
            };

            _context.Studies.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<StudyDTO> FindAsync(int id)
        {
            var study = await _context.Studies.FindAsync(id);

            if (study == null) return null;

            return new StudyDTO
            {
                Id = study.Id,
                Title = study.Title,
                Description = study.Description
            };
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

        public async Task<bool> UpdateAsync(StudyDTO study)
        {
            var entity = await _context.Studies.FindAsync(study.Id);

            if (entity == null) return false;

            entity.Title = study.Title;
            entity.Description = study.Description;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var study = await _context.Studies.FindAsync(id);

            if (study == null) return false;

            _context.Studies.Remove(study);

            await _context.SaveChangesAsync();

            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) _context.Dispose();

                
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
