using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorImplementation.Models
{
    public class FilterModel
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }

        public string LogicalOperator { get; set; }
    }
}
