using TvProgram = TvProgramDB.Core.Entities.Program;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TvProgramDB.Core.Interfaces;
using TvProgramDB.WebApi.DTO;
using TvProgramDB.Core.Entities;
using TvProgramDB.WebApi.Adapters;
using System;
using TvProgramDB.WebApi.Specifications;

namespace TvProgramDB.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProgramController : ControllerBase
    {
        private IRepository<TvProgram> _ProgramRepository;

        public ProgramController(IRepository<TvProgram> ProgramRepository)
        {
            _ProgramRepository = ProgramRepository;
        }

  

        // GET api/values
        [HttpGet()]
        public IEnumerable<ProgramDTO> GetRange(
            int startIndex = 0,
            int take=10,
            string chanel=null,
            DateTime? date = null,
            string type=null)
        {
            ProgramType programType;
            ProgramType? nullableProgramType = null;
            if (Enum.TryParse(type, out programType))
                nullableProgramType = programType;

            return _ProgramRepository
                .List(new GetTvProgramSpecification(chanel, date, nullableProgramType),startIndex, take, $"{nameof(TvProgram.Chanel)}.{nameof(Chanel.Country)}")
                .Select(c => ProgramAdapter.ToDTO(c));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Program = _ProgramRepository
                .GetById(id, nameof(TvProgram.Chanel), $"{nameof(TvProgram.Chanel)}.{nameof(Chanel.Country)}");
            if (Program == null)
                return NotFound();
            return Ok(ProgramAdapter.ToDTO(Program));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var Program = _ProgramRepository
                .GetById(id);

            if (Program != null)
                _ProgramRepository.Delete(Program);
        }
    }
}
