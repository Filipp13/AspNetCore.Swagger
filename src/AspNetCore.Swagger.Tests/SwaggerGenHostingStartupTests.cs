using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace AspNetCore.SwaggerGen.Tests
{
    [TestFixture(Category = "Integration")]
    public class SwaggerGenHostingStartupTests
    {
        private void ConfigureServices(IServiceCollection services) => services.AddControllers();

        private void Configure(IApplicationBuilder app) => app.UseRouting()
          .UseEndpoints(endpoints =>
          {
              endpoints.MapControllers();
          });

        [Test]
        public async Task Configure_ReturnsOpenApiDoc()
        {
            using var host = await new HostBuilder()
              .ConfigureWebHost(webBuilder =>
              {
                  webBuilder
              .UseTestServer()
              .UseSwaggerGenStartupAssembly()
              .ConfigureServices(ConfigureServices)
              .Configure(Configure);
              })
              .StartAsync();

            var response = await host.GetTestClient().GetAsync("/swagger/v1/swagger.json");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Configure_WithProductionEnv_Returns_HTTP_OK()
        {
            using var host = await new HostBuilder()
              .ConfigureWebHost(webBuilder =>
              {
                  webBuilder
              .UseTestServer()
              .UseSetting(WebHostDefaults.EnvironmentKey, "Test")
              .UseSwaggerGenStartupAssembly()
              .ConfigureServices(ConfigureServices)
              .Configure(Configure);
              })
              .StartAsync();

            var response = await host.GetTestClient().GetStringAsync("/swagger/v1/swagger.json");

            Assert.IsNotEmpty(response);
        }
    }
}