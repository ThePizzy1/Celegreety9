using Celegreety9.Features.TalentPricings.Interfaces;
using Celegreety9.Features.TalentPricings.Models;
using MediatR;

namespace Celegreety9.Features.TalentPricings.Queries
{
   
    public class GetTalentPricingQuery : IRequest<GetTalentPricingResult>
    {
        public int TalentId { get; set; }
        public int HistoryLimit { get; set; } = 10;
    }


    public class GetTalentPricingResult
    {
        public TalentPricing Current { get; set; }
        public IEnumerable<PricingHistory> History { get; set; } = new List<PricingHistory>();
    }


    public class GetTalentPricingHandler : IRequestHandler<GetTalentPricingQuery, GetTalentPricingResult>
    {
        private readonly ITalentPricingRepository _repo;

        public GetTalentPricingHandler(ITalentPricingRepository repo)
        {
            _repo = repo;
        }

        public async Task<GetTalentPricingResult> Handle(GetTalentPricingQuery request, CancellationToken cancellationToken)
        {
            var current = await _repo.GetTalentPricingAsync(request.TalentId);
            if (current == null)
                throw new KeyNotFoundException("Talent not found.");

            var history = await _repo.GetPricingHistoryAsync(request.TalentId, request.HistoryLimit);

            return new GetTalentPricingResult
            {
                Current = current,
                History = history
            };
        }
    }
}
