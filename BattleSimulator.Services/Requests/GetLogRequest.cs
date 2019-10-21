using BattleSimulator.Services.Responses;
using MediatR;

namespace BattleSimulator.Services.Requests
{
    public class GetLogRequest : RequestBase, IRequest<GetLogResponse>
    {
        public int BattleId { get; set; }
    }
}
