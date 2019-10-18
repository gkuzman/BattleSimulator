using BattleSimulator.Entities.BattleDTOs;
using BattleSimulator.Entities.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IBattleProcessor : ITransientService
    {
        Task<(ArmyDTO, BattleLog)> AttackAsync(ArmyDTO attacker, List<ArmyDTO> armies, int battleId, string jobId, CancellationToken cancellationToken);
    }
}
