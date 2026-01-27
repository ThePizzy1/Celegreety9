namespace Celegreety9.Features.TalentPricings.Models
{
    public class PricingHistory
    {
        public int Id { get; set; }
        public int TalentId { get; set; }
        public int PersonalPrice { get; set; }
        public int BusinessPrice { get; set; }
        public string StripeProductId { get; set; }
        public string StripePersonalPriceId { get; set; }
        public string StripeBusinessPriceId { get; set; }
        public string ChangeReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
