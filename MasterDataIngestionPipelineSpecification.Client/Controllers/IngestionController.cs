using MasterDataIngestionPipelineSpecification.Api.Dtos;
using MasterDataIngestionPipelineSpecification.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataIngestionPipelineSpecification.Client.Controllers
{
    public class IngestionController : Controller
    {
        private readonly IApiService _api;

        public IngestionController(IApiService api) => _api = api;

        // ── Customer ──────────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Customer() => View(new CustomerIngestRequest());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Customer(CustomerIngestRequest request)
        {
            var result = await _api.IngestCustomerAsync(request);

            if (result == null)
            {
                TempData["Error"] = "Failed to connect to the API.";
                return View(request);
            }

            if (result.Status == "Success")
            {
                TempData["Success"] = $"Customer data queued. Reference ID: {result.ReferenceId}";
                return RedirectToAction("Customer");
            }

            TempData["Error"] = result.Message;
            ViewBag.ValidationErrors = result.ValidationErrors;
            return View(request);
        }

        // ── Item ──────────────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Item() => View(new ItemIngestRequest { Material = new MaterialDto() });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Item(ItemIngestRequest request)
        {
            var result = await _api.IngestItemAsync(request);

            if (result == null)
            {
                TempData["Error"] = "Failed to connect to the API.";
                return View(request);
            }

            if (result.Status == "Success")
            {
                TempData["Success"] = $"Item data queued. Reference ID: {result.ReferenceId}";
                return RedirectToAction("Item");
            }

            TempData["Error"] = result.Message;
            ViewBag.ValidationErrors = result.ValidationErrors;
            return View(request);
        }
    }
}
