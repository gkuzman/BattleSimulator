using BattleSimulator.Entities.Enums;

namespace BattleSimulator.Entities.DB
{
    public class Army
    {
        public int BattleId { get; set; }
        public string Name { get; set; }
        public int Units { get; set; }
        public virtual Battle Battle { get; set; }
        public Strategy AttackStrategy { get; set; }
    }
}
