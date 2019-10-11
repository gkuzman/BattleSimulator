using BattleSimulator.Entities.Enums;
using BattleSimulator.Services.Responses;
using MediatR;

namespace BattleSimulator.Services.Requests
{
    public class AddArmyRequest : RequestBase, IRequest<AddArmyResponse>
    {
        public string Name { get; set; }
        public int Units { get; set; }
        public Strategy Strategy { get; set; }
    }
}
