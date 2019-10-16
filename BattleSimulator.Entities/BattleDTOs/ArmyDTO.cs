using BattleSimulator.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Entities.BattleDTOs
{
    public class ArmyDTO
    {
        public string Name { get; set; }
        public int Units { get; set; }
        public TimeSpan ReloadTimeTotal { get; set; }
        public TimeSpan ElapsedReloadTime { get; set; }
        public string TargetName { get; set; }
        public Strategy AttackStrategy { get; set; }
    }
}
