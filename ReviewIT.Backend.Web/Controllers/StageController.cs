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
    public class StageController : Controller
    {
        private readonly IStageRepository _repository;
        private readonly ILogger<StageController> _log;

        public StageController(IStageRepository repository, ILogger<StageController> log)
        {
            _repository = repository;
            _log = log;
        }

        // GET api/stage
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stages = await _repository.ReadAsync();

            if (stages.Count == 0) _log.LogDebug("No stages found. Returns empty list");

            return Ok(stages);
        }

        // GET api/stage/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var stage = await _repository.FindAsync(id);

            if (stage == null)
            {
                _log.LogDebug("No studies found. Return 404 NotFound");
                return NotFound();
            }

            return Ok(stage);
        }

        // POST api/stage
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StageNoIdDTO stage)
        {
            if (!ModelState.IsValid)
            {
                _log.LogWarning("ModelState not valid. Return BadRequest");
                return BadRequest(ModelState);
            }

            var id = await _repository.CreateAsync(stage);

            return CreatedAtAction(nameof(Get), new { id }, null);
        }

        // PUT api/stage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StageDTO stage)
        {
            if (id != stage.Id)
            {
                _log.LogWarning("id={id} does not match stage.Id={stage.Id}.", id, stage.Id);
                ModelState.AddModelError(string.Empty, "id is not matching stage.Id");
            }
            if (!ModelState.IsValid)
            {
                _log.LogWarning("ModelState not valid. Return BadRequest");
                return BadRequest(ModelState);
            }

            var wasUpdated = await _repository.UpdateAsync(stage);

            if (!wasUpdated)
            {
                _log.LogDebug("stage={stage} not found. Return 404 NotFound", stage);
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/stage/5
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