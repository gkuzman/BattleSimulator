using System;
using System.Collections.Generic;
using System.Text;

namespace BattleSimulator.Entities.Options
{
    public class BattleOptions
    {
        public int MinimumArmies { get; set; }
        public int HitPercentagePerArmyUnit { get; set; }
        public decimal DamagePerArmyUnit { get; set; }
        public TimeSpan ArmyReloadPerUnit { get; set; }
    }
}
