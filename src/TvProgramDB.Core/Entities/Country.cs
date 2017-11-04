using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TvProgramDB.Core.Entities;

namespace TvProgramDB.Core.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
    }
}
