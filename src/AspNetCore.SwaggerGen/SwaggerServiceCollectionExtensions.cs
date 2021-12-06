using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.SwaggerGen
{
    /// <summary>
    ///   Предоставляет методы расширения для добавления в DI контейнер <see cref="IServiceCollection" />
    ///   предварительно настроенных служб
    ///   <see cref="Swashbuckle" />.<see cref="Swashbuckle.AspNetCore" />.<see cref="Swashbuckle.AspNetCore.SwaggerGen" />.
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        private const string SwaggerOptionsSection = "Swagger";

        private const string SwaggerGenOptionsSection = "SwaggerGenerator";

        /// <summary>
        ///   Добавляет службы <see cref="Swashbuckle.AspNetCore.SwaggerGen" />
        ///   с настроенными "securityDefinitions" и "global security requirement" на схему Bearer.
        /// </summary>
        /// <param name="services">Ссылка на объект <see cref="IServiceCollection" />.</param>
        /// <param name="environment">Ссылка на объект <see cref="IWebHostEnvironment" />.</param>
        /// <param name="configuration">Ссылка на объект <see cref="IConfiguration" />.</param>
        /// <returns>Ссылка на объект <see cref="IServiceCollection" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///   Возникает если значение любого параметра равно <see langword="null" />.
        /// </exception>
        public static IServiceCollection AddSwagger(
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
              .AddSwaggerGen(options => GetXmlDocFilePhysicalPath(options, environment))
              .Configure<SwaggerOptions>(configuration.GetSection(SwaggerOptionsSection))
              .Configure<SwaggerGenOptions>(ConfigureSwaggerGenSecurity)
              .Configure<SwaggerGeneratorOptions>(configuration.GetSection(SwaggerGenOptionsSection))
              .Configure<SwaggerGeneratorOptions>(ConfigureSwaggerDocVersion)
              .AddTransient<IStartupFilter, SwaggerStartupFilter>();
        }

        private static void ConfigureSwaggerGenSecurity(SwaggerGenOptions options)
        {
            const string schemaName = "Bearer";

            var bearerSchema = new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Введите токен доступа в поле (без Bearer).",
                Name = "Authorization",
                Scheme = "Bearer",
                Type = SecuritySchemeType.Http,
                Reference = new OpenApiReference { Id = schemaName, Type = ReferenceType.SecurityScheme }
            };

            options.AddSecurityDefinition(schemaName, bearerSchema);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement { { bearerSchema, Array.Empty<string>() } });
        }

        private static void ConfigureSwaggerDocVersion(SwaggerGeneratorOptions options)
        {
            foreach (var (_, info) in options.SwaggerDocs)
            {
                info.Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0";
            }
        }

        private static void GetXmlDocFilePhysicalPath(SwaggerGenOptions options, IHostEnvironment environment)
        {
            string[] xmlFiles = Directory.GetFiles(environment.ContentRootPath, "*.xml", SearchOption.AllDirectories);

            foreach (var xmlFile in xmlFiles)
            {
                options.IncludeXmlComments(xmlFile, true);
            }
        }
    }
}