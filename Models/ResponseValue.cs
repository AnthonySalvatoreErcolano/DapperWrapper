using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperWrapper.Models
{
    public enum ResponseValue
    {
        Success = 0,
        Failed = 1,
        NotFound = 2,
        Invalid = 3,
        Unauthorized = 4,
        Warning = 5
    }
}
