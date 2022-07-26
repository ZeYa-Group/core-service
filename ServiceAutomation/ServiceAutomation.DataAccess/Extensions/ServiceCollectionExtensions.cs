﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceAutomation.DataAccess.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutomation.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly IDatabaseSecret databaseSecret = new DatabaseSecret();

        public static void ConfigureDataAccessServices(this IServiceCollection services,
            Action<DataAccessOptions> optionsSetter = default)
        {
            services.AddSingleton(databaseSecret);
            services.AddDbContext<PotgreSqlContext>(options =>
            {
                var dataAccessOptions = new DataAccessOptions();

                optionsSetter?.Invoke(dataAccessOptions);

                var connectionString = databaseSecret.GetConnectionString();
                var collectSqlQueries = dataAccessOptions.CollectSqlQueries;


                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly("MxtrAutomation.DataAccess.Migrations.Npgsql"));
                
            });

            services.AddScoped<ServiceDbContext, PotgreSqlContext>();
        }
    }

    public class DataAccessOptions
    {
        public bool CollectSqlQueries { get; set; }
    }
}
