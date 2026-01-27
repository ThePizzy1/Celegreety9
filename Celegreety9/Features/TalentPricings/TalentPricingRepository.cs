namespace Celegreety9.Features.TalentPricings
{
    using Celegreety9.Features.TalentPricings.Interfaces;
    using Celegreety9.Features.TalentPricings.Models;
    using Dapper;
    using Npgsql;

    public class TalentPricingRepository : ITalentPricingRepository
    {
        private readonly IConfiguration _config;

        public TalentPricingRepository(IConfiguration config)
        {
            _config = config;
        }

        private NpgsqlConnection GetConnection() =>
            new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task<int> UpsertTalentPricingAsync(TalentPricing pricing)
        {
            using var conn = GetConnection();
            var id = await conn.ExecuteScalarAsync<int>(
                "SELECT public.fn_upsert_talent_pricing(@TalentId, @StripeProductId, @PersonalPrice, @BusinessPrice, @StripePersonalPriceId, @StripeBusinessPriceId)",
                pricing);
            return id;
        }

        public async Task InsertPricingHistoryAsync(PricingHistory history)
        {
            using var conn = GetConnection();
            await conn.ExecuteAsync(
                "SELECT public.fn_insert_pricing_history(@TalentId, @PersonalPrice, @BusinessPrice, @StripeProductId, @StripePersonalPriceId, @StripeBusinessPriceId, @ChangeReason)",
                history);
        }

        public async Task<TalentPricing> GetTalentPricingAsync(int talentId)
        {
            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<TalentPricing>(
                "SELECT * FROM public.talent_profiles WHERE talent_id = @TalentId", new { TalentId = talentId });
        }

        public async Task<IEnumerable<PricingHistory>> GetPricingHistoryAsync(int talentId, int limit = 10)
        {
            using var conn = GetConnection();
            return await conn.QueryAsync<PricingHistory>(
                "SELECT * FROM public.pricing_history WHERE talent_id = @TalentId ORDER BY created_at DESC LIMIT @Limit",
                new { TalentId = talentId, Limit = limit });
        }
    }

}
