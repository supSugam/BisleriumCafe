using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Interfaces
{
    public interface ITaskResponse
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
    }
}
