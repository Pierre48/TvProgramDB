using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvProgramDB.Core.Entities;
using TvProgramDB.Core.Interfaces;
using TvProgramDB.WebApi.DTO;
using TvProgramDB.WebApi.Adapters;
using System.Net;

namespace TvProgramDB.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ChanelController : ControllerBase
    {
        private IRepository<Chanel> _chanelRepository;

        public ChanelController(IRepository<Chanel> chanelRepository)
        {
            _chanelRepository = chanelRepository;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<ChanelDTO> Get()
        {
            return _chanelRepository
                .ListAll(nameof(Chanel.Country))
                .Select(c=>ChanelAdapter.ToDTO(c));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var chanel = _chanelRepository
                .GetById(id, nameof(Chanel.Country));
            if (chanel == null)
                return NotFound();
            return Ok(ChanelAdapter.ToDTO(chanel));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var chanel = _chanelRepository
                .GetById(id);

            if (chanel != null)
                _chanelRepository.Delete(chanel);
        }
    }
}
