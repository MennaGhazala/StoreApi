﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ErrorDetails
    {
        public string ErrorMessage { get; set; }
        public int StatuCode { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }

  
}
