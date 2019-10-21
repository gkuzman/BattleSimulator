using BattleSimulator.Services.Responses;
using MediatR;

namespace BattleSimulator.Services.Requests
{
    public class StartGameRequest : RequestBase, IRequest<StartGameResponse>
    {
        public bool Reset { get; set; }
    }
}
