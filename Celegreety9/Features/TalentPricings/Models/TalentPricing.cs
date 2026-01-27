namespace Celegreety9.Features.TalentPricings.Models
{
    public class TalentPricing
    {
        public int TalentId { get; set; }
        public string StageName { get; set; }
        public int PersonalPrice { get; set; }
        public int BusinessPrice { get; set; }
        public string StripeProductId { get; set; }
        public string StripePersonalPriceId { get; set; }
        public string StripeBusinessPriceId { get; set; }
        public DateTime PricesLastSyncedAt { get; set; }
    }
}
