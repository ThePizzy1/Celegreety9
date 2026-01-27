using System;

namespace Celegreety9.Features.TalentPricings.Models
{
    public class TalentProfile
    {
        public int Id { get; set; }
        public int TalentId { get; set; }
        public string StageName { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? StripeProductId { get; set; }
        public int? PersonalPrice { get; set; }
        public int? BusinessPrice { get; set; }
        public string? StripePersonalPriceId { get; set; }
        public string? StripeBusinessPriceId { get; set; }
        public DateTime PricesLastSyncedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
