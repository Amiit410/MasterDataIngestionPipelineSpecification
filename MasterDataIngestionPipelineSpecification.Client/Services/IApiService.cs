using MasterDataIngestionPipelineSpecification.Api.Dtos;

namespace MasterDataIngestionPipelineSpecification.Client.Services
{
    public interface IApiService
    {
        Task<DashboardSummaryDto?> GetDashboardSummaryAsync();
        Task<PagedResult<IngestionLogDto>?> GetLogsAsync(string domain, int page, int pageSize);
        Task<IngestResponse?> IngestCustomerAsync(CustomerIngestRequest request);
        Task<IngestResponse?> IngestItemAsync(ItemIngestRequest request);
    }
}
