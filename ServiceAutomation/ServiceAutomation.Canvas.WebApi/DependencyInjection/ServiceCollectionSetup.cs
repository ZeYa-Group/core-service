using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceAutomation.DataAccess;
using ServiceAutomation.DataAccess.Extensions;

namespace ServiceAutomation.Canvas.WebApi.DependencyInjection
{
    public abstract class ServiceCollectionSetup
    {
        public virtual IServiceCollection Configure(IConfiguration configuration, IServiceCollection services)
        {
            SetupDatabase(services);
            
            return services;
        }

        public virtual void SetupDatabase(IServiceCollection services)
        {
            services.ConfigureDataAccessServices((o) => o.CollectSqlQueries = false);
        }
    }

    public class WebApiServiceCollectionSetup : ServiceCollectionSetup
    {

    }
}
