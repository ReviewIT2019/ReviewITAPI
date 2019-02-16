using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReviewIT.Backend.Common.DTOs;
using ReviewIT.Backend.Models.Repositories;
using System.Threading.Tasks;

namespace ReviewIT.Backend.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudyController : Controller
    {
        private readonly IStudyRepository _repository;
        private readonly ILogger<StudyController> _log;

        public StudyController(IStudyRepository repository, ILogger<StudyController> log)
        {
            _repository = repository;
            _log = log;
        }

        // GET api/study
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var studies = await _repository.ReadAsync();

            if (studies.Count == 0) _log.LogDebug("No studies found. Returns empty list");

            return Ok(studies);
        }

        // GET api/study/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var study = await _repository.FindAsync(id);

            if (study == null)
            {
                _log.LogDebug("No studies found. Return 404 NotFound");
                return NotFound();
            }

            return Ok(study);
        }

        // POST api/study
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StudyNoIdDTO study)
        {
            if (!ModelState.IsValid)
            {
                _log.LogWarning("ModelState not valid. Return BadRequest");
                return BadRequest(ModelState);
            }

            var id = await _repository.CreateAsync(study);

            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        // PUT api/study/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StudyDTO study)
        {
            if (id != study.Id)
            {
                _log.LogWarning("id={id} does not match study.Id={study.Id}.", id, study.Id);
                ModelState.AddModelError(string.Empty, "id is not matching study.Id");
            }
            if (!ModelState.IsValid)
            {
                _log.LogWarning("ModelState not valid. Return BadRequest");
                return BadRequest(ModelState);
            }

            var wasUpdated = await _repository.UpdateAsync(study);

            if (!wasUpdated)
            {
                _log.LogDebug("study={study} not found. Return 404 NotFound", study);
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/study/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var wasDeleted = await _repository.DeleteAsync(id);

            if (!wasDeleted)
            {
                _log.LogDebug("id={id} not found. Return 404 NotFound", id);
                return NotFound();
            }

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _repository.Dispose();

            base.Dispose(disposing);
        }
    }
}
