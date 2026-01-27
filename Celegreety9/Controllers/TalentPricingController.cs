using Celegreety9.Features.TalentPricings.Interfaces;
using Celegreety9.Features.TalentPricings.Models;
using Celegreety9.Features.TalentPricings.Service;
using Microsoft.AspNetCore.Mvc;

namespace Celegreety9.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TalentPricingController : ControllerBase
    {
        private readonly ITalentPricingRepository _repo;
        private readonly StripeService _stripe;

        public TalentPricingController(ITalentPricingRepository repo, StripeService stripe)
        {
            _repo = repo;
            _stripe = stripe;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TalentPricing pricing)
        {
            if (pricing.BusinessPrice < pricing.PersonalPrice)
                return BadRequest("Business price must be >= personal price.");

            var productId = await _stripe.CreateProduct(pricing.TalentId, pricing.StageName);

            var personalPriceId = await _stripe.CreatePrice(productId, pricing.PersonalPrice, "eur", "personal");
            var businessPriceId = await _stripe.CreatePrice(productId, pricing.BusinessPrice, "eur", "business");

            pricing.StripeProductId = productId;
            pricing.StripePersonalPriceId = personalPriceId;
            pricing.StripeBusinessPriceId = businessPriceId;
            pricing.PricesLastSyncedAt = DateTime.UtcNow;

            await _repo.UpsertTalentPricingAsync(pricing);
            await _repo.InsertPricingHistoryAsync(new PricingHistory
            {
                TalentId = pricing.TalentId,
                PersonalPrice = pricing.PersonalPrice,
                BusinessPrice = pricing.BusinessPrice,
                StripeProductId = productId,
                StripePersonalPriceId = personalPriceId,
                StripeBusinessPriceId = businessPriceId,
                ChangeReason = "Initial creation"
            });

            return Ok(pricing);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TalentPricing pricing)
        {
            if (pricing.BusinessPrice < pricing.PersonalPrice)
                return BadRequest("Business price must be >= personal price.");

            var existing = await _repo.GetTalentPricingAsync(pricing.TalentId);

            if (existing == null)
                return NotFound("Talent not found.");

            await _stripe.ArchivePrice(existing.StripePersonalPriceId);
            await _stripe.ArchivePrice(existing.StripeBusinessPriceId);

            var personalPriceId = await _stripe.CreatePrice(existing.StripeProductId, pricing.PersonalPrice, "eur", "personal");
            var businessPriceId = await _stripe.CreatePrice(existing.StripeProductId, pricing.BusinessPrice, "eur", "business");

            existing.PersonalPrice = pricing.PersonalPrice;
            existing.BusinessPrice = pricing.BusinessPrice;
            existing.StripePersonalPriceId = personalPriceId;
            existing.StripeBusinessPriceId = businessPriceId;
            existing.PricesLastSyncedAt = DateTime.UtcNow;

            await _repo.UpsertTalentPricingAsync(existing);
  
            await _repo.InsertPricingHistoryAsync(new PricingHistory
            {
                TalentId = existing.TalentId,
                PersonalPrice = pricing.PersonalPrice,
                BusinessPrice = pricing.BusinessPrice,
                StripeProductId = existing.StripeProductId,
                StripePersonalPriceId = personalPriceId,
                StripeBusinessPriceId = businessPriceId,
              
            });

            return Ok(existing);
        }

        [HttpGet("{talentId}")]
        public async Task<IActionResult> Get(int talentId)
        {
            var current = await _repo.GetTalentPricingAsync(talentId);
            if (current == null)
                return NotFound("Talent not found.");

            var history = await _repo.GetPricingHistoryAsync(talentId, 10); 

            return Ok(new
            {
                Current = current,
                History = history
            });
        }
    }
}
