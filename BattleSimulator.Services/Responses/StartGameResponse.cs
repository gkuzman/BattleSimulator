using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Services.Responses
{
    public class StartGameResponse : ResponseBase
    {
        public int BattleId { get; set; }
    }
}
