using BattleSimulator.Entities.BattleDTOs;
using BattleSimulator.Entities.DB;
using BattleSimulator.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BattleSimulator.Services.Services
{
    public class DbEntitiesToDtosMapper : IDbEntitiesToDtosMapper
    {
        public List<ArmyDTO> MapBattleLogToArmiesDtoList(BattleLog log)
        {
            if (string.IsNullOrEmpty(log?.BattleSnapshot))
            {
                throw new Exception("There's an error reading the battle log. Battlesnapshot is empty!");
            }

            return JsonConvert.DeserializeObject<List<ArmyDTO>>(log.BattleSnapshot);
        }

        public List<ArmyDTO> MapDbArmiesToArmiesDtoList(List<Army> armies)
        {
            var result = new List<ArmyDTO>();
            foreach (var army in armies)
            {
                var dto = new ArmyDTO
                {
                    AttackStrategy = army.AttackStrategy,
                    ElapsedReloadTime = TimeSpan.Zero,
                    ReloadTimeTotal = TimeSpan.Zero,
                    Name = army.Name,
                    Target = null,
                    Units = army.Units
                };
            }

            return result;
        }
    }
}
