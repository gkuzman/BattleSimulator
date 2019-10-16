using BattleSimulator.Entities.BattleDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IBattleProcessor : ITransientService
    {
        Task<ArmyDTO> Attack(ArmyDTO attacker, List<ArmyDTO> armies);
    }
}
