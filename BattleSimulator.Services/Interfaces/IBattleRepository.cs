using BattleSimulator.Entities.DB;
using BattleSimulator.Entities.Enums;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IBattleRepository : ITransientService
    {
        Task<Battle> GetInitializingBattleAsync();
        Task<BattleLog> GetBattleLog(int battleId, string jobId);
        Task<int> CreateBattleAsync();
        Task<bool> UpdateBattleAsync(int battleId, BattleStatus battleStatus, string jobId = "");
        Task<Battle> GetInProgressBattleAsync();
    }
}
