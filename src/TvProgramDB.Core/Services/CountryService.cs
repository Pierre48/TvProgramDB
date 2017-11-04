using System;
using System.Collections.Generic;
using System.Text;
using TvProgramDB.Core.Entities;
using TvProgramDB.Core.Interfaces;

namespace TvProgramDB.Core.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;

        public CountryService(IRepository<Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public IEnumerable<Country> GetAll()
        {
            return _countryRepository.ListAll();
        }

        public void Create(string name)
        {
            _countryRepository.Add(new Country { Name = name });
        }
    }
}
