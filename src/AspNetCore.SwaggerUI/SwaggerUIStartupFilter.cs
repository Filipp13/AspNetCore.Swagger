using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SP.Core.AspNetCore.SwaggerUI
{
    /// <summary>
    ///   Фильтр добавляет в конвейер промежуточного ПО <see cref="SwaggerUIMiddleware" />.
    /// </summary>
    /// <remarks>
    ///   Для конфигурации используйте <see cref="SwaggerUIOptions" /> с любым поставщиком конфигурации.
    /// </remarks>
    /// <example>
    ///   {
    ///   "SwaggerUI": {
    ///   "ConfigObject": {
    ///   "Urls": [
    ///   {
    ///   "Name": "OpenApi Spec for some Web-API service."
    ///   "Url": "http://localhost:5000/swagger/v1/swagger.json"
    ///   }
    ///   ]
    ///   }
    ///   }
    ///   }
    /// </example>
    public class SwaggerUIStartupFilter : IStartupFilter
    {
        /// <summary>
        ///   Инициализирует новый экземпляр класса <see cref="SwaggerUIStartupFilter" />.
        /// </summary>
        /// <param name="hostEnvironment">Ссылка на объект <see cref="IWebHostEnvironment" />.</param>
        public SwaggerUIStartupFilter(IWebHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
        }

        private IWebHostEnvironment HostEnvironment { get; }

        /// <summary>
        ///   Если <see cref="IWebHostEnvironment" />.<see cref="IWebHostEnvironment.EnvironmentName" /> !=
        ///   <see cref="Environments" />.<see cref="Environments.Production" />,
        ///   тогда добавляет в конец конвейера промежуточно ПО <see cref="SwaggerUIMiddleware" />,
        ///   иначе не выполняет действий.
        /// </summary>
        /// <param name="next">Ссылка на объект <see cref="Action{T}" /> - делегат для настройки конвейера промежуточного ПО.</param>
        /// <returns>Ссылка на объект <see cref="Action{T}" /> - делегат для настройки конвейера промежуточного ПО.</returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                if (HostEnvironment.IsProduction())
                {
                    next(builder);
                }
                else
                {
                    next(builder);
                    builder.UseSwaggerUI();
                }
            };
        }
    }
}