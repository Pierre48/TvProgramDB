using System;
using System.Collections.Generic;
using System.Text;
using TvProgramDB.Core.Entities;

namespace TvProgramDB.Core.Specifications
{
    class ProgramStartNearSpecification : SpecificationBase<Program>
    {
        public ProgramStartNearSpecification(int chanelId,DateTime startDate)
                : base(i => i.Chanel.Id == chanelId && i.StartDate>= startDate.AddMinutes(-10) && i.StartDate <= startDate.AddMinutes(10))
            {
        }
    }
}
