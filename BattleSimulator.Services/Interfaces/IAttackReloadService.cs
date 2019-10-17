using BattleSimulator.Entities.BattleDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IAttackReloadService : ITransientService
    {
        Task ReloadAsync(ArmyDTO attacker, CancellationToken cancellationToken);
    }
}
