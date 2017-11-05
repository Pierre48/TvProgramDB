using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvProgramDB.Core.Entities;

namespace TvProgramDB.WebApi.DTO
{
    public class ProgramDTO
    {
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public DateTime EndDate { get; internal set; }
        public string ChannelName { get; internal set; }
        public string CountryCode { get; internal set; }
        public string CountryName { get; internal set; }
        public DateTime StartDate { get; internal set; }
        public string Type { get; internal set; }
    }
}
