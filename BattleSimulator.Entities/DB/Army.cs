using BattleSimulator.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Entities.DB
{
    public class Army
    {
        public int BattleId { get; set; }
        public string Name { get; set; }
        public int Units { get; set; }
        public Battle Battle { get; set; }
        public Strategy AttackStrategy { get; set; }
    }
}
