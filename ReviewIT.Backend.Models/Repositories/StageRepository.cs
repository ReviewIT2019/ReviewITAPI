using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReviewIT.Backend.Common.DTOs;
using ReviewIT.Backend.Entities.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReviewIT.Backend.Entities.Entitities;

namespace ReviewIT.Backend.Models.Repositories
{
    public class StageRepository : IStageRepository
    {
        private readonly IContext _context;

        public StageRepository(IContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(StageNoIdDTO stage)
        {
            var entity = new Stage
            {
                Name = stage.Name,
                Description = stage.Description,
                StageInitiated = stage.StageInitiated,
                StudyId = stage.StudyId
            };

            _context.Stages.Add(entity);

            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<StageDTO> FindAsync(int id)
        {
            var stage = await _context.Stages.FindAsync(id);

            if (stage == null) return null;

            return new StageDTO
            {
                Id = stage.Id,
                Name = stage.Name,
                Description = stage.Description,
                StageInitiated = stage.StageInitiated,
                StudyId = stage.StudyId
            };
        }

        public async Task<IReadOnlyCollection<StageDTO>> ReadAsync()
        {
            var stages = from stage in _context.Stages
                         select new StageDTO
                         {
                             Id = stage.Id,
                             Name = stage.Name,
                             Description = stage.Description,
                             StageInitiated = stage.StageInitiated,
                             StudyId = stage.StudyId
                         };

            return await stages.ToListAsync();
        }

        public async Task<bool> UpdateAsync(StageDTO stage)
        {
            var entity = await _context.Stages.FindAsync(stage.Id);

            if (entity == null) return false;

            entity.Name = stage.Name;
            entity.Description = stage.Description;
            entity.StageInitiated = stage.StageInitiated;
            entity.StudyId = stage.StudyId;

            await _context.SaveChangesAsync();

            return true;

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var stage = await _context.Stages.FindAsync(id);

            if (stage == null) return false;

            _context.Stages.Remove(stage);

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
