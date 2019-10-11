using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Services.Responses
{
    public abstract class ResponseBase
    {
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public bool HasErrors => ErrorMessages.Count > 0;
    }
}
