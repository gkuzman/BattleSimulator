using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IBattleRepository : ITransientService
    {
        Task<int> GetInitializingBattleIdAsync();
        Task<int> CreateBattleAsync();
    }
}
