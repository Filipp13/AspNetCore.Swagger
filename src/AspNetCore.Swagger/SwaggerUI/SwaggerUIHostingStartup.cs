using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SP.Core.AspNetCore.SwaggerUI;

[assembly: HostingStartup(typeof(SwaggerUIHostingStartup))]

namespace SP.Core.AspNetCore.SwaggerUI
{
    /// <summary>
    ///   Представляет конфигурацию платформы,
    ///   которая будет применяться к IWebHostBuilder при построении IWebHost
    ///   и добавит UI Swagger для спецификации OpenAPI к Web-API службе.
    /// </summary>
    public class SwaggerUIHostingStartup : IHostingStartup
    {
        /// <summary>
        ///   Если <see cref="IWebHostEnvironment.EnvironmentName" /> != <see cref="Environments.Production" />,
        ///   тогда добавляет в конец <see cref="IServiceCollection" /> фильтр запуска <see cref="SwaggerUIStartupFilter" />,
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

                services.ConfigureSwaggerUI(context.HostingEnvironment, context.Configuration);
            });
        }
    }
}