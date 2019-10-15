using Hangfire.Server;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IGameService : ITransientService
    {
        Task StartGameAsync(PerformContext performContext, int battleId);
    }
}
