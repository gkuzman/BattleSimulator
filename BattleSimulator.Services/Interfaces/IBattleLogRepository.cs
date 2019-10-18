using BattleSimulator.Entities.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IBattleLogRepository : ITransientService
    {
        Task InsertBattleLogAsync(IEnumerable<BattleLog> battleLogs);
    }
}
