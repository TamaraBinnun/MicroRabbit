using System.Net.Mime;
using System.Text.Json;
using System.Text;
using MicroRabbit.Domain.Core.Interfaces;

namespace MicroRabbit.Infrastructure.Synchronous.Services
{
    public class HttpSender : ISynchronousSender
    {
        private readonly HttpClient _httpClient;

        public HttpSender(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<T>?> GetDataAsync<T>(IEnumerable<int> ids, string url) where T : class
        {
            var idsJson = new StringContent(
                 JsonSerializer.Serialize(ids),
                 Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using var httpResponseMessage = await _httpClient.GetAsync(url + $"/{idsJson}");

            httpResponseMessage.EnsureSuccessStatusCode();

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var response = await JsonSerializer.DeserializeAsync<IEnumerable<T>?>(contentStream, serializeOptions);
            return response;
        }

        public async Task<bool> UpdateDataAsync<T>(T entity, string url) where T : class
        {
            var entityJson = new StringContent(
                 JsonSerializer.Serialize(entity),
                 Encoding.UTF8,
                MediaTypeNames.Application.Json);

            using var httpResponseMessage = await _httpClient.PutAsync(url, entityJson);

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