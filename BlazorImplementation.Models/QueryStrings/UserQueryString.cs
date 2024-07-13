using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorImplementation.Models.QueryStrings
{
    public  class UserQueryString
    {
        public string? Filters { get; set; }
        public long? PageIndex { get; set; }
        public long? PageSize { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
}
