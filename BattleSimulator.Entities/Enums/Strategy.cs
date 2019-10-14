using System.ComponentModel;

namespace BattleSimulator.Entities.Enums
{
    public enum Strategy
    {
        [Description("None")]
        None,
        [Description("Random")]
        Random,
        [Description("Weakest")]
        Weakest,
        [Description("Strongest")]
        Strongest
    }
}
