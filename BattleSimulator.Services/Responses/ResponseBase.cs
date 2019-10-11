using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Services.Responses
{
    public abstract class ResponseBase
    {
        public List<string> Messages { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
