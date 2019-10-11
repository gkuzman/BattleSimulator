using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class AddArmyService : IRequestHandler<AddArmyRequest, AddArmyResponse>
    {
        public async Task<AddArmyResponse> Handle(AddArmyRequest request, CancellationToken cancellationToken)
        {
            var result = new AddArmyResponse();
            return result;
        }
    }
}
