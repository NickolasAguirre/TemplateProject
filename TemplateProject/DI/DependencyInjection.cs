using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateProject.Infrastructure.DatabaseContext;
//#if (userTracking)
using TemplateProject.API.CurrentUserImpl;
//#endif
//#if (autoDI)
using TemplateProject.Application;
using TemplateProject.Infrastructure;
//#endif
//#if (mapster)
using Mapster;
using MapsterMapper;
using TemplateProject.Application.Mapping;
using TemplateProject.Application.Services.GeneralServices.CurrentUserService;
//#endif

namespace TemplateProject.API.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            services.AddDbContext<TemplateProjectDbContext>(options =>
            {
                //#if (database == "sqlserver")
                options.UseSqlServer(connectionString);
                //#elif (database == "postgresql")
                options.UseNpgsql(connectionString);
                //#elif (database == "mysql")
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                //#else
                options.UseSqlite(connectionString ?? "Data Source=app.db");
                //#endif
            });
            return services;
        }

//#if (userTracking)
        public static IServiceCollection AddCurrentUser(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<CurrentUserService, CurrentUserImplementation>();
            return services;
        }
//#endif

//#if (autoDI)
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblies(typeof(IInfrastructureAssemblyMarker).Assembly)
                .AddClasses(c => c.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Scoped))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromAssemblies(typeof(IInfrastructureAssemblyMarker).Assembly)
                .AddClasses(c => c.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Transient))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

            services.Scan(scan => scan
                .FromAssemblies(typeof(IInfrastructureAssemblyMarker).Assembly)
                .AddClasses(c => c.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Singleton))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblies(typeof(IApplicationAssemblyMarker).Assembly)
                .AddClasses(c => c.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Scoped))
                .AsSelf()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromAssemblies(typeof(IApplicationAssemblyMarker).Assembly)
                .AddClasses(c => c.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Transient))
                .AsSelf()
                .WithTransientLifetime());

            services.Scan(scan => scan
                .FromAssemblies(typeof(IApplicationAssemblyMarker).Assembly)
                .AddClasses(c => c.WithAttribute<InjectableAttribute>(a => a.Lifetime == ServiceLifetime.Singleton))
                .AsSelf()
                .WithSingletonLifetime());

            return services;
        }
//#endif

//#if (mapster)
        public static IServiceCollection AddMapsterMapping(this IServiceCollection services)
        {
            var config = MapsterConfig.GetConfiguration();
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }
//#endif
    }
}
