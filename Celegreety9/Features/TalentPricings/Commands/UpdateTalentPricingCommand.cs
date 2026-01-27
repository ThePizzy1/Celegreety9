using Celegreety9.Features.TalentPricings.Interfaces;
using Celegreety9.Features.TalentPricings.Models;
using Celegreety9.Features.TalentPricings.Service;
using MediatR;

namespace Celegreety7.Features.TalentPricings.Commands
{
    public class UpdateTalentPricingCommand : IRequest<TalentPricing>
    {
        public int TalentId { get; set; }
        public int PersonalPrice { get; set; }
        public int BusinessPrice { get; set; }
        public string ChangeReason { get; set; } = "Updated prices";
    }

    public class UpdateTalentPricingHandler : IRequestHandler<UpdateTalentPricingCommand, TalentPricing>
    {
        private readonly ITalentPricingRepository _repo;
        private readonly StripeService _stripe;

        public UpdateTalentPricingHandler(ITalentPricingRepository repo, StripeService stripe)
        {
            _repo = repo;
            _stripe = stripe;
        }

        public async Task<TalentPricing> Handle(UpdateTalentPricingCommand request, CancellationToken cancellationToken)
        {
            if (request.BusinessPrice < request.PersonalPrice)
                throw new ArgumentException("Business price must be >= personal price.");

            var existing = await _repo.GetTalentPricingAsync(request.TalentId);
            if (existing == null) throw new KeyNotFoundException("Talent not found.");

            await _stripe.ArchivePrice(existing.StripePersonalPriceId);
            await _stripe.ArchivePrice(existing.StripeBusinessPriceId);

            var personalPriceId = await _stripe.CreatePrice(existing.StripeProductId, request.PersonalPrice, "eur", "personal");
            var businessPriceId = await _stripe.CreatePrice(existing.StripeProductId, request.BusinessPrice, "eur", "business");

            var pricing = new TalentPricing
            {
                TalentId = request.TalentId,
                StageName = existing.StageName,
                PersonalPrice = request.PersonalPrice,
                BusinessPrice = request.BusinessPrice,
                StripeProductId = existing.StripeProductId,
                StripePersonalPriceId = personalPriceId,
                StripeBusinessPriceId = businessPriceId,
                PricesLastSyncedAt = DateTime.UtcNow
            };

            await _repo.UpsertTalentPricingAsync(pricing);

            await _repo.InsertPricingHistoryAsync(new PricingHistory
            {
                TalentId = request.TalentId,
                PersonalPrice = request.PersonalPrice,
                BusinessPrice = request.BusinessPrice,
                StripeProductId = existing.StripeProductId,
                StripePersonalPriceId = personalPriceId,
                StripeBusinessPriceId = businessPriceId,
                ChangeReason = request.ChangeReason
            });

            return pricing;
        }
    }
}
