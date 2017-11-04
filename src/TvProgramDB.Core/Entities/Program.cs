using System;
using System.Collections.Generic;
using System.Text;

namespace TvProgramDB.Core.Entities
{
    public class Program : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProgramType ProgramType { get; set; }
        public Chanel Chanel { get; set; }
    }
}
