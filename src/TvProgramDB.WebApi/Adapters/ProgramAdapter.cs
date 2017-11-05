using TvProgramDB.WebApi.DTO;
using ProgramE = TvProgramDB.Core.Entities.Program;

namespace TvProgramDB.WebApi.Adapters
{
    public class ProgramAdapter
    {
        internal static ProgramDTO ToDTO(ProgramE p)
        {
            if (p == null) return null;
            return new ProgramDTO
            {
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ChannelName = p.Chanel?.Name,
                CountryCode = p.Chanel?.Country?.Code,
                CountryName = p.Chanel?.Country?.Name,
                Type = p.ProgramType.ToString()
            };
        }
    }
}
