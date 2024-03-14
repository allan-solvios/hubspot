using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cannolai.Hubspot.Utility
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? Result { get; set; }
    }

    public class Response
    {
        public IList<ResponseModel>? ResponseModel { get; set; } = [];
    }
}
