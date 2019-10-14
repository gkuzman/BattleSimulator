using BattleSimulator.Entities.DB;
using BattleSimulator.Services.Interfaces;
using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class AddArmyService : IRequestHandler<AddArmyRequest, AddArmyResponse>
    {
        private readonly IArmyRepository _armyRepository;

        public AddArmyService(IArmyRepository armyRepository)
        {
            _armyRepository = armyRepository;
        }
        public async Task<AddArmyResponse> Handle(AddArmyRequest request, CancellationToken cancellationToken)
        {
            var result = new AddArmyResponse();

            var army = new Army { AttackStrategy = request.Strategy, Name = request.Name, Units = request.Units };
            var transactionIsSuccessfull = await _armyRepository.AddAnArmyAsync(army);

            if (!transactionIsSuccessfull)
            {
                result.ErrorMessages.Add($"Failed to add an army with name {request.Name}. Please, try again later.");
            }

            return result;
        }
    }
}
