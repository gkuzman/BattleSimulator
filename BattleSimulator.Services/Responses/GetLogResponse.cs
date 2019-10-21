using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Services.Responses
{
    public class GetLogResponse : ResponseBase
    {
        public string FullActionLog { get; set; }

        public string LastSnapshotLog { get; set; }

        public int Id { get; set; }

        public int BattleId { get; set; }

        public string JobId { get; set; }
    }
}
