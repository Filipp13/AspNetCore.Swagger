using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SP.Core.AspNetCore.SwaggerUI
{
    /// <summary>
    ///   Предоставляет методы расширения для добавления в DI контейнер <see cref="IServiceCollection" />
    ///   предварительно настроенных служб
    ///   <see cref="Swashbuckle" />.<see cref="Swashbuckle.AspNetCore" />.<see cref="Swashbuckle.AspNetCore.SwaggerGen" />.
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        private const string ConfigurationSection = "SwaggerUI";

        /// <summary>
        ///   Конфигурирует <see cref="Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions" />,
        ///   считывая конфигурацию из поставщиков конфигурации.
        ///   Используйте секцию <see cref="ConfigurationSection"/> для определения опций.
        /// </summary>
        /// <param name="services">Ссылка на объект <see cref="IServiceCollection" />.</param>
        /// <param name="environment">Ссылка на объект <see cref="IWebHostEnvironment" />.</param>
        /// <param name="configuration">Ссылка на объект <see cref="IConfiguration" />.</param>
        /// <returns>Ссылка на объект <see cref="IServiceCollection" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///   Возникает если значение любого параметра равно <see langword="null" />.
        /// </exception>
        public static IServiceCollection ConfigureSwaggerUI(
          this IServiceCollection services,
          IWebHostEnvironment environment,
          IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return services
              .Configure<SwaggerUIOptions>(configuration.GetSection(ConfigurationSection))
              .AddTransient<IStartupFilter, SwaggerUIStartupFilter>();
        }
    }
}