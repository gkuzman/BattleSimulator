using BattleSimulator.Entities.BattleDTOs;
using BattleSimulator.Entities.DB;
using BattleSimulator.Entities.Options;
using BattleSimulator.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        private readonly IBattleLogRepository _logRepository;
        private ArmyDTO _attacker;
        private CancellationToken _cancellationToken;
        private readonly StringBuilder _sb = new StringBuilder();

        public BattleProcessor(ILogger<BattleProcessor> logger, IOptions<BattleOptions> options, IAttackReloadService attackReloadService, IBattleLogRepository logRepository)
        {
            _logger = logger;
            _options = options;
            _reloadService = attackReloadService;
            _logRepository = logRepository;
        }
        public async Task<ArmyDTO> AttackAsync(ArmyDTO attacker, List<ArmyDTO> armies, int battleId, string jobId, CancellationToken cancellationToken)
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

            await _reloadService.ReloadAsync(_attacker, cancellationToken);
            await SaveBattleLogAsync(armies, battleId, jobId);
            return ClearState(attacker);
        }

        private async Task SaveBattleLogAsync(List<ArmyDTO> armies, int battleId, string jobId)
        {
            var battleLog = new BattleLog
            {
                ActionTaken = _sb.ToString(),
                BattleId = battleId,
                JobId = jobId,
                LogTime = DateTime.UtcNow,
                BattleSnapshot = JsonConvert.SerializeObject(armies)
            };

            await _logRepository.InsertBattleLogAsync(battleLog, _cancellationToken);

            _sb.Clear();
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
                var attackDamage = GetAttackDamage();

                var startAttackLog = $"{_attacker.Name} started attacking {target.Name} with {target.Units} units.";
                _logger.LogInformation(startAttackLog);
                _sb.AppendLine(startAttackLog);

                if (IsAttackSuccessful())
                {
                    var logSuccessfulAttack = $"{_attacker.Name} successfully attacks {target.Name}. {target.Name} takes {attackDamage} damage.";
                    _sb.AppendLine(logSuccessfulAttack);
                    _logger.LogInformation(logSuccessfulAttack);
                    target.Units -= attackDamage;
                }
                else
                {
                    var logFailedAttack = $"{_attacker.Name} failed to attack {target.Name}. Attack missed!";
                    _sb.AppendLine(logFailedAttack);
                    _logger.LogInformation(logFailedAttack);
                }
            }
        }

        private int GetAttackDamage()
        {
            var damage = _attacker.Units * _options.Value.DamagePerArmyUnit;
            return (int)Math.Floor(damage);
        }

        private bool IsAttackSuccessful()
        {
            var rnd = new Random();
            var roll = rnd.Next(1, 101);

            return _attacker.Units * _options.Value.HitPercentagePerArmyUnit >= roll;
        }

        private ArmyDTO ClearState(ArmyDTO attacker)
        {
            attacker.TargetName = string.Empty;

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
