using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cannolai.Hubspot.Entity
{
    public class ContactDTO
    {
        public string? HsContactId { get; set; }
        public List<PropertiesDto> Properties { get; set; } = [];
    }
}
