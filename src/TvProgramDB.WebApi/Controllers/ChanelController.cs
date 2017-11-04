using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvProgramDB.Core.Entities;
using TvProgramDB.Core.Interfaces;

namespace TvProgramDB.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ChanelController : Controller
    {
        private IRepository<Chanel> _chanelRepository;

        public ChanelController(IRepository<Chanel> chanelRepository)
        {
            _chanelRepository = chanelRepository;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Chanel> Get()
        {
            return _chanelRepository.ListAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Chanel Get(int id)
        {
            return _chanelRepository.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
