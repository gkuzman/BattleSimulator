using BattleSimulator.Entities.DB;
using BattleSimulator.Entities.BattleDTOs;
using System.Collections.Generic;

namespace BattleSimulator.Services.Interfaces
{
    public interface IDbEntitiesToDtosMapper : ITransientService
    {
        List<ArmyDTO> MapBattleLogToArmiesDtoList(BattleLog log);
        List<ArmyDTO> MapDbArmiesToArmiesDtoList(List<Army> armies);
    }
}
