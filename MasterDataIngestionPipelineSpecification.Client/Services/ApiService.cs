using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MasterDataIngestionPipelineSpecification.Api.Dtos;

namespace MasterDataIngestionPipelineSpecification.Client.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _factory;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public ApiService(IHttpClientFactory factory) => _factory = factory;

        private HttpClient CreateClient()
        {
            var client = _factory.CreateClient("MasterDataAPI");
            return client;
        }

        public async Task<DashboardSummaryDto?> GetDashboardSummaryAsync()
        {
            var client = CreateClient();
            Console.WriteLine(client.BaseAddress);
            var response = await client.GetAsync("api/v1/dashboard/summary");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DashboardSummaryDto>(json, _jsonOptions);
        }

        public async Task<PagedResult<IngestionLogDto>?> GetLogsAsync(string domain, int page, int pageSize)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/v1/dashboard/logs?Domain={domain}&Page={page}&PageSize={pageSize}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResult<IngestionLogDto>>(json, _jsonOptions);
        }

        public async Task<IngestResponse?> IngestCustomerAsync(CustomerIngestRequest request)
        {
            var client = CreateClient();
            var body = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/v1/customers/ingest", body);
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IngestResponse>(json, _jsonOptions);
        }

        public async Task<IngestResponse?> IngestItemAsync(ItemIngestRequest request)
        {
            var client = CreateClient();
            var body = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/v1/items/ingest", body);
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IngestResponse>(json, _jsonOptions);
        }
    }
}
