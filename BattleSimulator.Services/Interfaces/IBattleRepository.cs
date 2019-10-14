using BattleSimulator.Entities.DB;
using BattleSimulator.Entities.Enums;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IBattleRepository : ITransientService
    {
        Task<Battle> GetInitializingBattleAsync();
        Task<int> CreateBattleAsync();
        Task<bool> ChangeBattleStatus(int battleId, BattleStatus battleStatus);
    }
}
