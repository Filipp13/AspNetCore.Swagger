using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SP.Core.AspNetCore.SwaggerUI
{
    /// <summary>
    ///   Предоставляет расширения для подключения <see cref="SwaggerUIHostingStartup" />.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        ///   Если <see cref="IWebHostEnvironment.EnvironmentName" /> != <see cref="Environments.Production" />,
        ///   тогда получает текущее значение ключа <see cref="WebHostDefaults.HostingStartupAssembliesKey" />,
        ///   в конфигурации узла и добавляет к нему <see cref="System.Reflection.Assembly.FullName" />
        ///   из определение типа <see cref="SwaggerUIHostingStartup" />, иначе не выполняет действий.
        /// </summary>
        /// <param name="builder">Ссылка на объект <see cref="IWebHostBuilder" />.</param>
        /// <returns>Ссылка на объект <see cref="IWebHostBuilder" />.</returns>
        public static IWebHostBuilder UseSwaggerUIStartupAssembly(this IWebHostBuilder builder)
        {
            if (builder.GetSetting(WebHostDefaults.EnvironmentKey) == Environments.Production)
            {
                return builder;
            }

            var currentValue = builder.GetSetting(WebHostDefaults.HostingStartupAssembliesKey);

            return builder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey,
              string.IsNullOrWhiteSpace(currentValue)
                ? $"{typeof(SwaggerUIHostingStartup).Assembly.FullName};"
                : $"{currentValue}{typeof(SwaggerUIHostingStartup).Assembly.FullName};");
        }
    }
}