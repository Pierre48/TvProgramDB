using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvProgramDB.Core.Entities;
using TvProgramDB.Core.Specifications;
using TvProgram = TvProgramDB.Core.Entities.Program;

namespace TvProgramDB.WebApi.Specifications
{
    public class GetTvProgramSpecification : SpecificationBase<TvProgram>
    {

        public GetTvProgramSpecification(string chanel, DateTime? date,  ProgramType? type)
            : base(x => 
                (chanel==null || string.Equals(x.Chanel.Name,chanel,StringComparison.CurrentCultureIgnoreCase)) &&
                (date==null || (x.StartDate <= date && date <=x.EndDate))&&
                (type==null || (type == x.ProgramType))
            )
        {
        }
    }
}
