﻿using BattleSimulator.Entities.DB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Interfaces
{
    public interface IArmyRepository : ITransientService
    {
        Task<bool> AddAnArmyAsync(Army request);
        Task<bool> IsArmyAddingBlockedAsync();
        Task<List<Army>> GetArmiesAsync(int battleId);
    }
}
