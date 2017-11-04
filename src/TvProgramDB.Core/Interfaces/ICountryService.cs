using System;
using System.Collections.Generic;
using System.Text;
using TvProgramDB.Core.Entities;

namespace TvProgramDB.Core.Services
{
    public interface ICountryService
    {
        IEnumerable<Country> GetAll();

        void Create(string name);
    }
}
