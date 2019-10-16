using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Entities.DB
{
    public class BattleLog
    {
        public DateTime LogTime { get; set; }

        public int Id { get; set; }

        public int BattleId { get; set; }

        public string JobId { get; set; }

        public Battle Battle { get; set; }

        public string ActionTaken { get; set; }

        public string BattleSnapshot { get; set; }
    }
}
