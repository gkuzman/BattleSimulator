using BattleSimulator.Entities.Enums;
using System.Collections.Generic;

namespace BattleSimulator.Entities.DB
{
    public class Battle
    {
        public int Id { get; set; }
        public virtual IEnumerable<Army> Armies { get; set; }
        public BattleStatus BattleStatus { get; set; }
        public string JobId { get; set; }
    }
}
