using Celegreety9.Features.TalentPricings.Interfaces;
using Celegreety9.Features.TalentPricings.Models;
using Dapper;
using System.Data;

namespace Celegreety9.Features
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> RegisterUserAsync(string name, string email)
        {
            return await _db.ExecuteScalarAsync<int>(
                "SELECT public.fn_register_user(@Name, @Email)",
                new { Name = name, Email = email }
            );
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM public.fn_get_user_by_email(@Email)",
                new { Email = email }
            );
        }
    }
}
