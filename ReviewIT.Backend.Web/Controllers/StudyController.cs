using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReviewIT.Backend.Common.DTOs;
using ReviewIT.Backend.Models.Repositories;

namespace ReviewIT.Backend.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyController : ControllerBase
    {
        private readonly IStudyRepository _repository;

        public StudyController(IStudyRepository repository)
        {
            _repository = repository;
        }

        // GET api/study
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var studies = await _repository.ReadAsync();

            return Ok(studies);
        }

        // GET api/study/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/study
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/study/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/study/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
