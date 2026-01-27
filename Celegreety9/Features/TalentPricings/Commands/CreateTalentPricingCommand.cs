namespace Celegreety9.Features.TalentPricings.Commands
{
    using Celegreety9.Features.TalentPricings.Interfaces;
    using Celegreety9.Features.TalentPricings.Models;
    using Celegreety9.Features.TalentPricings.Service;
    using MediatR;


  
    public class CreateTalentPricingCommand : IRequest<TalentPricing>
    {
        public int TalentId { get; set; }
        public string StageName { get; set; }
        public int PersonalPrice { get; set; }
        public int BusinessPrice { get; set; }
        public string Currency { get; set; } = "EUR";
    }

  
    public class CreateTalentPricingHandler : IRequestHandler<CreateTalentPricingCommand, TalentPricing>
    {
        private readonly ITalentPricingRepository _repo;
        private readonly StripeService _stripe;

        public CreateTalentPricingHandler(ITalentPricingRepository repo, StripeService stripe)
        {
            _repo = repo;
            _stripe = stripe;
        }

        public async Task<TalentPricing> Handle(CreateTalentPricingCommand request, CancellationToken cancellationToken)
        {
            if (request.BusinessPrice < request.PersonalPrice)
                throw new ArgumentException("Business price must be >= personal price.");

            var productId = await _stripe.CreateProduct(request.TalentId, request.StageName);
            var personalPriceId = await _stripe.CreatePrice(productId, request.PersonalPrice, request.Currency, "personal");
            var businessPriceId = await _stripe.CreatePrice(productId, request.BusinessPrice, request.Currency, "business");

            var pricing = new TalentPricing
            {
                TalentId = request.TalentId,
                StageName = request.StageName,
                PersonalPrice = request.PersonalPrice,
                BusinessPrice = request.BusinessPrice,
                StripeProductId = productId,
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
                StripeProductId = productId,
                StripePersonalPriceId = personalPriceId,
                StripeBusinessPriceId = businessPriceId,
                ChangeReason = "Initial creation"
            });

            return pricing;
        }
    }


}
