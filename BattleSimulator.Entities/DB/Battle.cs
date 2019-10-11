using BattleSimulator.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Entities.DB
{
    public class Battle
    {
        public int Id { get; set; }
        public List<Army> Armies { get; set; }
        public BattleStatus BattleStatus { get; set; }
    }
}
