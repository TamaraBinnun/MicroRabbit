using MicroRabbit.Domain.Core.Dtos;
using MicroRabbit.Orders.Application.Dtos;
using MicroRabbit.Orders.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace MicroRabbit.Orders.Infrastructure.Synchronous.Services
{
    public class HttpMicroRabbitBooksClient : IMicroRabbitBooksClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpMicroRabbitBooksClient(HttpClient httpClient,
                                     IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<IEnumerable<BookUnits>?> GetBookUnitsInStockAsync(List<int> bookIds)
        {
            var bookIdsJson = new StringContent(
                 JsonSerializer.Serialize(bookIds),
                 Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using var httpResponseMessage = await _httpClient.GetAsync(_config["MicroRabbitBooks:StockApi"] + $"/{bookIdsJson}");

            httpResponseMessage.EnsureSuccessStatusCode();

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = await JsonSerializer.DeserializeAsync<IEnumerable<BookUnits>?>(contentStream, serializeOptions);
            return response;
        }

        public async Task<bool> UpdateBookUnitsInStockAsync(List<BookUnits> bookUnits)
        {
            var bookUnitsJson = new StringContent(
                 JsonSerializer.Serialize(bookUnits),
                 Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using var httpResponseMessage = await _httpClient.PutAsync(_config["MicroRabbitBooks:StockApi"], bookUnitsJson);

            httpResponseMessage.EnsureSuccessStatusCode();

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var response = await JsonSerializer.DeserializeAsync<int>(contentStream, serializeOptions);
            return (response > 0);
        }
    }
}