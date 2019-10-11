using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Services.Requests
{
    public abstract class RequestBase
    {
        public List<string> Messages { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
