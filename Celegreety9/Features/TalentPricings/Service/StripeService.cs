namespace Celegreety9.Features.TalentPricings.Service
{
    using Stripe;

    public class StripeService
    {
        private readonly string _apiKey;

        public StripeService(IConfiguration config)
        {
            _apiKey = config["Stripe:ApiKey"];
            StripeConfiguration.ApiKey = _apiKey;
        }

        public async Task<string> CreateProduct(int talentId, string talentName)
        {
            var service = new ProductService();
            var product = await service.CreateAsync(new ProductCreateOptions
            {
                Name = talentName,
                Metadata = new Dictionary<string, string>
            {
                { "talent_id", talentId.ToString() },
                { "type", "talent_booking" }
            }
            });
            return product.Id;
        }

        public async Task<string> CreatePrice(string productId, long amount, string currency, string priceType)
        {
            var service = new PriceService();
            var price = await service.CreateAsync(new PriceCreateOptions
            {
                Product = productId,
                UnitAmount = amount,
                Currency = currency,
                Metadata = new Dictionary<string, string> { { "price_type", priceType } }
            });
            return price.Id;
        }

        public async Task ArchivePrice(string priceId)
        {
            var service = new PriceService();
            await service.UpdateAsync(priceId, new PriceUpdateOptions { Active = false });
        }
    }

}
