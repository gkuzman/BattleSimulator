using BattleSimulator.Entities.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IBattleLogRepository : ITransientService
    {
        Task InsertBattleLogAsync(IEnumerable<BattleLog> battleLogs);
        Task<BattleLog> GetLatestLogForBattle(int battleId);
        Task<List<string>> GetActionLogsForBattle(int battleId, string jobId);
    }
}
