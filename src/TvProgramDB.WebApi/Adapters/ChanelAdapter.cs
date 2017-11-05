using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvProgramDB.Core.Entities;
using TvProgramDB.WebApi.DTO;

namespace TvProgramDB.WebApi.Adapters
{
    internal static class ChanelAdapter
    {
        internal static ChanelDTO ToDTO(Chanel c)
        {
            if (c == null) return null;
            return new ChanelDTO
            {
                Name = c.Name,
                CountryCode = c.Country?.Code,
                CountryName = c.Country?.Name,
            };
        }
    }
}
