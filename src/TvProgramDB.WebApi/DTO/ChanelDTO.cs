using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TvProgramDB.WebApi.DTO
{
    public class ChanelDTO
    {
        public string Name { get; internal set; }
        public string CountryCode { get; internal set; }
        public string CountryName { get; internal set; }
    }
}
