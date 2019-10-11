using BattleSimulator.Services.Responses;
using MediatR;

namespace BattleSimulator.Services.Requests
{
    public class AddArmyRequest : RequestBase, IRequest<AddArmyResponse>
    {
    }
}
