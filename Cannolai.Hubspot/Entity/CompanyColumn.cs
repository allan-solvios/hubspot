using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cannolai.Hubspot.Entity
{
    public class CompanyColumn
    {
        public string? name { get; set; }
        public string? label { get; set; }
        public string? description { get; set; }
        public string? groupName { get; set; }
        public string? type { get; set; }
        public string? fieldType { get; set; }
        public bool formField { get; set; } = true;
        public bool hidden { get; set; } = false;
        public int? displayOrder { get; set; } = -1;
        public List<Option>? options { get; set; } = [];
        public bool hasUniqueValue { get; set; } = false;
        public bool isRequired { get; set; } = false;
    }
}
