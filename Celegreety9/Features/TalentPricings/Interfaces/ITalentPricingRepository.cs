using Celegreety9.Features.TalentPricings.Models;

namespace Celegreety9.Features.TalentPricings.Interfaces
{
    public interface ITalentPricingRepository
    {
        Task<int> UpsertTalentPricingAsync(TalentPricing pricing);
        Task InsertPricingHistoryAsync(PricingHistory history);
        Task<TalentPricing> GetTalentPricingAsync(int talentId);
        Task<IEnumerable<PricingHistory>> GetPricingHistoryAsync(int talentId, int limit = 10);
    }
}
