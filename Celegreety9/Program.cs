using System.Data;
using Npgsql;
using Celegreety9.Features.TalentPricings.Interfaces;
using Celegreety9.Features.TalentPricings.Service;
using Celegreety9.Features.TalentPricings;
using Celegreety9.Features;

namespace Celegreety9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();


            builder.Services.AddOpenApi();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy
                            .WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddScoped<IDbConnection>(sp =>
                new NpgsqlConnection(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITalentPricingRepository, TalentPricingRepository>();

            builder.Services.AddScoped<StripeService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

           
            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
