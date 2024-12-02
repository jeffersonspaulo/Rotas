using Microsoft.EntityFrameworkCore;
using Rotas.API.Data;
using Rotas.API.Data.Interfaces;
using Rotas.API.Data.Repositories;
using Rotas.API.Services;
using Rotas.API.Services.Interfaces;

namespace Rotas.API.Configurations
{
    public static class ApplicationConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IRotaService, RotaService>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        }
    }
}
