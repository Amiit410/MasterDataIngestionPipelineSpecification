using MasterDataIngestionPipelineSpecification.Api.Dtos;

namespace MasterDataIngestionPipelineSpecification.Api.Service
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<PagedResult<IngestionLogDto>> GetLogsAsync(LogQueryRequest request);
    }
}
