using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
