using Microsoft.OpenApi.Models;
using ClaimManagement.Logger;
using ClaimManagement.Data.Services;
using ClamManagement.Data;
using ClaimManagement.Data.Repo;

namespace ClamManagement.Helper
{
    internal static class ServiceCollection
    {


        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {

            return services
               .AddScoped<IClaimRepo,ClaimRepo>()


            ;//end of repositories
        }
        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IClaimService,ClaimService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
            ;//end of services
        }



        internal static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                string[] methodsOrder = new string[] { "get", "post", "put", "patch", "delete", "options", "trace" };
                options.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{Array.IndexOf(methodsOrder, apiDesc.HttpMethod.ToLower())}");
                options.CustomSchemaIds(type => type.ToString());
                

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Policy Api",

                });
                

            });
        }

        internal static ILoggingBuilder AddDbLogger(this ILoggingBuilder builder, Action<DbLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
