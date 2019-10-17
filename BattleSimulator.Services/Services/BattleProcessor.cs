using BattleSimulator.Entities.BattleDTOs;
using BattleSimulator.Entities.Options;
using BattleSimulator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class BattleProcessor : IBattleProcessor
    {
        private readonly ILogger<BattleProcessor> _logger;
        private readonly IOptions<BattleOptions> _options;
        private readonly IAttackReloadService _reloadService;
        private ArmyDTO _attacker;
        private CancellationToken _cancellationToken;

        public BattleProcessor(ILogger<BattleProcessor> logger, IOptions<BattleOptions> options, IAttackReloadService attackReloadService)
        {
            _logger = logger;
            _options = options;
            _reloadService = attackReloadService;
        }
        public async Task<ArmyDTO> Attack(ArmyDTO attacker, List<ArmyDTO> armies, CancellationToken cancellationToken)
        {
            if (attacker.Units < 1)
            {
                return attacker;
            }

            _cancellationToken = cancellationToken;
            _attacker = attacker;

            if (cancellationToken.IsCancellationRequested)
            {
                return ClearState(attacker);
            }

            InitiateAttack(armies);

            
            _logger.LogInformation($"{_attacker.Name} finished");
            await _reloadService.ReloadAsync(_attacker, cancellationToken);
            return ClearState(attacker);
        }

        private void InitiateAttack(List<ArmyDTO> armies)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return;
            }

            // game over
            if (armies.Count(x => x.Units > 0) < 2)
            {
                return;
            }

            AssignTarget(armies);
            // if target name is empty it means that battle is over based on assignation criteria
            if (string.IsNullOrEmpty(_attacker.TargetName))
            {
                return;
            }

            var target = armies.FirstOrDefault(x => x.Name == _attacker.TargetName);

            if (target != null && target.Units > 0)
            {
                _logger.LogInformation($"{_attacker.Name} started attacking {_attacker.TargetName}");
                target.Units -= 5;
            }
        }

        private ArmyDTO ClearState(ArmyDTO attacker)
        {
            attacker.TargetName = string.Empty;
            attacker.ReloadTimeTotal = TimeSpan.Zero;
            attacker.ElapsedReloadTime = TimeSpan.Zero;

            return attacker;
        }

        private void AssignTarget(List<ArmyDTO> armies)
        {
            // if it is not empty it means that we have loaded from battlelog on crash
            if (string.IsNullOrEmpty(_attacker.TargetName))
            {
                switch (_attacker.AttackStrategy)
                {
                    case Entities.Enums.Strategy.Random:
                        _attacker.TargetName = armies.Where(a => a.Units > 0 && a.Name != _attacker.Name)?.OrderByDescending(x => x.Units).FirstOrDefault()?.Name;
                        break;
                    case Entities.Enums.Strategy.Weakest:
                        _attacker.TargetName = armies.Where(a => a.Units > 0 && a.Name != _attacker.Name)?.OrderBy(x => x.Units).FirstOrDefault()?.Name;
                        break;
                    case Entities.Enums.Strategy.Strongest:
                        _attacker.TargetName = armies.Where(a => a.Units > 0 && a.Name != _attacker.Name)?.OrderBy(x => Guid.NewGuid()).FirstOrDefault()?.Name;
                        break;
                    case Entities.Enums.Strategy.None:
                    default:
                        break;
                }
            }
        }
    }
}
