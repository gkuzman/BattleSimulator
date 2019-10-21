using BattleSimulator.Services.Interfaces;
using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class GetLogService : IRequestHandler<GetLogRequest, GetLogResponse>
    {
        private readonly IBattleLogRepository _battleLogRepository;
        private readonly StringBuilder _sb = new StringBuilder();

        public GetLogService(IBattleLogRepository battleLogRepository)
        {
            _battleLogRepository = battleLogRepository;
        }
        public async Task<GetLogResponse> Handle(GetLogRequest request, CancellationToken cancellationToken)
        {
            var response = new GetLogResponse();
            var latestLog = await _battleLogRepository.GetLatestLogForBattle(request.BattleId);

            if (latestLog is null)
            {
                response.ErrorMessages.Add($"Could not find a battle log for battle with id {request.BattleId}");
                return response;
            }

            var fullActionLogs = await _battleLogRepository.GetActionLogsForBattle(latestLog.BattleId, latestLog.JobId);
            response.Id = latestLog.Id;
            response.JobId = latestLog.JobId;
            response.BattleId = latestLog.BattleId;
            response.LastSnapshotLog = latestLog.BattleSnapshot;

            foreach (var log in fullActionLogs ?? new List<string>())
            {
                _sb.AppendLine(log);
            }

            response.FullActionLog = _sb.ToString();

            return response;
        }
    }
}
