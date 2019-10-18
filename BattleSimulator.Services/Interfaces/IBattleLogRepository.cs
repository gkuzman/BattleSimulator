using BattleSimulator.Entities.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IBattleLogRepository : ITransientService
    {
        Task<IEnumerable<BattleLog>> InsertBattleLogAsync(IEnumerable<BattleLog> battleLogs);
    }
}
