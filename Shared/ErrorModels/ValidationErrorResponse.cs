using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ValidationErrorResponse
    {
        public string ErrorMessage { get; set; }
        public int StatuCode { get; set; }
        public IEnumerable<ValidationError>? Errors { get; set; }


    }
    public class ValidationError
    {
        public string Key { get; set; }
        public IEnumerable<string> Errors { get; set; }



    }
}
