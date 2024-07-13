using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorImplementation.Models
{
    public class CountryModel
    {
        public long CountryId { get; set; }
        public string CountryName { get; set; } = null!;
        public string ISO { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public DateTime DeletedAt { get; set; }

    }
}
