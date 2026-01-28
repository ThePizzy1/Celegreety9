using Celegreety9.Features.TalentPricings.Models;

namespace Celegreety9.Features.TalentPricings.Interfaces
{
    public interface IUserRepository
    {
        Task<int> RegisterUserAsync(string name, string email);
        Task<User?> GetByEmailAsync(string email);
    }
}
