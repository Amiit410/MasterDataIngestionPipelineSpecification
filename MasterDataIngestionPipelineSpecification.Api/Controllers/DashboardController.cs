using MasterDataIngestionPipelineSpecification.Api.Dtos;
using MasterDataIngestionPipelineSpecification.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataIngestionPipelineSpecification.Api.Controllers
{
    [ApiController]
    [Route("api/v1/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboard;

        public DashboardController(IDashboardService dashboard) => _dashboard = dashboard;

        /// <summary>Returns high-level KPI summary for both domains.</summary>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(DashboardSummaryDto), 200)]
        public async Task<IActionResult> GetSummary()
            => Ok(await _dashboard.GetSummaryAsync());

        /// <summary>Returns paginated ingestion log with domain filter (Customer | Item).</summary>
        [HttpGet("logs")]
        [ProducesResponseType(typeof(PagedResult<IngestionLogDto>), 200)]
        public async Task<IActionResult> GetLogs([FromQuery] LogQueryRequest request)
            => Ok(await _dashboard.GetLogsAsync(request));
    }
}
