using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace SP.Core.AspNetCore.SwaggerUI.Tests
{
    [TestFixture(Category = "Integration")]
    public class SwaggerUIHostingStartupTests
    {
        private void ConfigureServices(IServiceCollection services) => services.AddControllers();

        private void Configure(IApplicationBuilder app) => app.UseRouting()
          .UseEndpoints(endpoints =>
          {
              endpoints.MapControllers();
          });

        [Test]
        public async Task Configure_WithProductionEnv_Return_HTTP_NotFound()
        {
            using var host = await new HostBuilder()
              .ConfigureWebHost(webBuilder =>
              {
                  webBuilder
              .UseTestServer()
              .UseSwaggerUIStartupAssembly()
              .ConfigureServices(ConfigureServices)
              .Configure(Configure);
              })
              .StartAsync();

            var response = await host.GetTestClient().GetAsync("/swagger");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task Configure_WithNonProductionEnv_Return_HTTP_Moved()
        {
            using var host = await new HostBuilder()
              .ConfigureWebHost(webBuilder =>
              {
                  webBuilder
              .UseTestServer()
              .UseSetting(WebHostDefaults.EnvironmentKey, "Test")
              .UseSwaggerUIStartupAssembly()
              .ConfigureServices(ConfigureServices)
              .Configure(Configure);
              })
              .StartAsync();

            var response = await host.GetTestClient().GetAsync("/swagger");

            Assert.AreEqual(HttpStatusCode.Moved, response.StatusCode);
        }

        [Test]
        public async Task Configure_WithNonProductionEnv_Return_HTTP_OK()
        {
            using var host = await new HostBuilder()
              .ConfigureWebHost(webBuilder =>
              {
                  webBuilder
              .UseTestServer()
              .UseSetting(WebHostDefaults.EnvironmentKey, "Test")
              .UseSwaggerUIStartupAssembly()
              .ConfigureServices(ConfigureServices)
              .Configure(Configure);
              })
              .StartAsync();

            var response = await host.GetTestClient().GetAsync("http://localhost/swagger/index.html");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}