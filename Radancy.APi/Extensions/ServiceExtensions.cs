using Core.CandidateService;
using Data;
using Data.Candidate;
using Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Radancy.APi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RadancyDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHttpContextAccessor();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            return services;
        }
    }
}