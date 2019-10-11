using BattleSimulator.DAL.Contexts;
using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Services
{
    public class AddArmyService : IRequestHandler<AddArmyRequest, AddArmyResponse>
    {
        private NonTrackingContext _context;

        public AddArmyService(NonTrackingContext context)
        {
            _context = context;
        }
        public async Task<AddArmyResponse> Handle(AddArmyRequest request, CancellationToken cancellationToken)
        {
            var e = await _context.Battles.FindAsync(1);
            var c = await _context.Armies.FindAsync("pera", 1);
            var result = new AddArmyResponse();
            return result;
        }
    }
}
