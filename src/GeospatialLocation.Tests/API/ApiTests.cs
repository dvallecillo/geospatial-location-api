using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using GeospatialLocation.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GeospatialLocation.Tests.API
{
    public class ApiTests
    {
        private readonly HttpClient _client;

        public ApiTests()
        {
            // Arrange
            var server = new TestServer(
                new WebHostBuilder()
                    .UseKestrel()
                    .ConfigureServices(s => s.AddAutofac())
                    .UseStartup<Startup>()
                    .UseSetting(
                        "Redis:Host",
                        "localhost:6379"));
            _client = server.CreateClient();
        }

        [Fact]
        public async Task FindsExistingResults()
        {
            // Act
            var response = await _client.GetAsync(
                "/Locations?Location.Latitude=52.2165425&Location.Longitude=5.4778534&MaxDistance=1000&MaxResults=10");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("AH Frieswijkstraat 72, Nijkerk", responseString);
        }

        [Fact]
        public async Task RequestIntMaxValueResults()
        {
            // Act
            var response = await _client.GetAsync(
                $"/Locations?Location.Latitude=52.2165425&Location.Longitude=5.4778534&MaxDistance={int.MaxValue}&MaxResults={int.MaxValue}");
            response.EnsureSuccessStatusCode();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task FindsNothingOnEdges()
        {
            // Act
            var response = await _client.GetAsync(
                "/Locations?Location.Latitude=90&Location.Longitude=180&MaxDistance=10000&MaxResults=10");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("[]", responseString);
        }

        [Fact]
        public async Task RespondsBadRequestWhenZeroResultsRequested()
        {
            // Act
            var response = await _client.GetAsync(
                "/Locations?Location.Latitude=90&Location.Longitude=180&MaxDistance=100000&MaxResults=0");
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}