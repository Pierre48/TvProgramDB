using System;
using System.Collections.Generic;
using System.Text;

namespace TvProgramDB.Core.Entities
{
    public class Chanel : BaseEntity
    {
        public string Name { get; set; }
        public Country Country { get; set; }
        public List<ChanelName> ChanelNames { get; set; }
    }
}
