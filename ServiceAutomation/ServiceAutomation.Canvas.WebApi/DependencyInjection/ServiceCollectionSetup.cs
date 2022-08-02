using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceAutomaion.Services.Interfaces;
using ServiceAutomaion.Services.Services;
using ServiceAutomation.Canvas.AutoMapping;
using ServiceAutomation.DataAccess;
using ServiceAutomation.DataAccess.Extensions;


namespace ServiceAutomation.Canvas.WebApi.DependencyInjection
{
    public abstract class ServiceCollectionSetup
    {
        public virtual IServiceCollection Configure(IConfiguration configuration, IServiceCollection services)
        {
            SetupDatabase(services);
            SetupMapper(services);
            SetupServices(services);

            return services;
        }

        public virtual void SetupDatabase(IServiceCollection services)
        {
            services.ConfigureDataAccessServices((o) => o.CollectSqlQueries = false);
        }

        public virtual void SetupMapper(IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfile<MappingProfile>();
            });
        }

        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IIdentityGenerator,IdentityGenerator>();
        }
    }

    public class WebApiServiceCollectionSetup : ServiceCollectionSetup
    {

    }
}
