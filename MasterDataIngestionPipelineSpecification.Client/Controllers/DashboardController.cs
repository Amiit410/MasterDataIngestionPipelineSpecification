using MasterDataIngestionPipelineSpecification.Api.Dtos;
using MasterDataIngestionPipelineSpecification.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataIngestionPipelineSpecification.Client.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IApiService _api;

        public DashboardController(IApiService api) => _api = api;

        public async Task<IActionResult> Index()
        {
            var summary = await _api.GetDashboardSummaryAsync();
            return View(summary ?? new DashboardSummaryDto());
        }

        public async Task<IActionResult> Logs(string domain = "Customer", int page = 1)
        {
            var result = await _api.GetLogsAsync(domain, page, 20);
            ViewBag.Domain = domain;
            return View(result ?? new PagedResult<IngestionLogDto>());
        }

        [HttpGet]
        public IActionResult RawPayload(string data)
        {
            ViewBag.Json = data;
            return View();
        }
    }
}
