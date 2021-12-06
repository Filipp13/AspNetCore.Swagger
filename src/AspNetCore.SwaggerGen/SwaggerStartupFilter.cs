using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace AspNetCore.SwaggerGen
{
    /// <summary>
    ///   Фильтр добавляет в конвейер промежуточного ПО <see cref="SwaggerMiddleware" />.
    /// </summary>
    public class SwaggerStartupFilter : IStartupFilter
    {
        /// <summary>
        ///   Инициализирует новый экземпляр класса <see cref="SwaggerStartupFilter" />.
        /// </summary>
        /// <param name="hostEnvironment">Ссылка на объект <see cref="IWebHostEnvironment" />.</param>
        public SwaggerStartupFilter(IWebHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
        }

        private IWebHostEnvironment HostEnvironment { get; }

        /// <summary>
        ///   Если <see cref="IWebHostEnvironment.EnvironmentName" /> != <see cref="Environments.Production" />,
        ///   тогда добавляет в конец конвейера промежуточно ПО <see cref="SwaggerMiddleware" />,
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
                    builder.UseSwagger();
                }
            };
        }
    }
}