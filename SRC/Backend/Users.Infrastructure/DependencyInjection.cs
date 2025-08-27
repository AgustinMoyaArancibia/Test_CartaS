using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Infrastructure.Data;


namespace Users.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string 'Default' no configurada.");

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString, b =>
                {
                    // asegura que las migraciones se generen aquí
                    b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                });
            });

            // acá podés registrar repos: services.AddScoped<IVentaRepository, VentaRepository>();

            return services;
        }
    }
}
