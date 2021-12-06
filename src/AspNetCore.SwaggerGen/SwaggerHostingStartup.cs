using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspNetCore.SwaggerGen;

[assembly: HostingStartup(typeof(SwaggerHostingStartup))]

namespace AspNetCore.SwaggerGen
{
    /// <summary>
    ///   Представляет конфигурацию платформы,
    ///   которая будет применяться к IWebHostBuilder при построении IWebHost
    ///   и добавит генератор спецификации OpenAPI (Swagger) к Web-API службе.
    /// </summary>
    public class SwaggerHostingStartup : IHostingStartup
    {
        /// <summary>
        ///   Если <see cref="IWebHostEnvironment.EnvironmentName" /> != <see cref="Environments.Production" />,
        ///   тогда добавляет в конец <see cref="IServiceCollection" /> фильтр запуска <see cref="SwaggerStartupFilter" />,
        ///   иначе не выполняет действий.
        /// </summary>
        /// <param name="builder">Ссылка на объект <see cref="IWebHostBuilder" />.</param>
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                if (context.HostingEnvironment.IsProduction())
                {
                    return;
                }

                services.AddSwagger(context.HostingEnvironment, context.Configuration);
            });
        }
    }
}