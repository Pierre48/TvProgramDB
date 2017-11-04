using System;
using System.Collections.Generic;
using System.Text;
using TvProgramDB.Core.Entities;

namespace TvProgramDB.Core.Specifications
{
    class CountryCodeSpecification : SpecificationBase<Country>
    {
            public CountryCodeSpecification(string code)
                : base(i => (code!=null && code==i.Code))
            {
            }
        }
    }
