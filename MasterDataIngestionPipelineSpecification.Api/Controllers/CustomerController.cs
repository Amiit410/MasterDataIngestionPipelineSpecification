using System.Text.Json;
using MasterDataIngestionPipelineSpecification.Api.Data;
using MasterDataIngestionPipelineSpecification.Api.Dtos;
using MasterDataIngestionPipelineSpecification.Api.Entitties;
using MasterDataIngestionPipelineSpecification.Api.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataIngestionPipelineSpecification.Api.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidationService _validationService;

        public CustomerController(AppDbContext context,IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        [HttpPost("ingest")]
        public async Task<IActionResult> Ingest([FromBody] CustomerIngestRequest request)
        {
            //var errors = _validationService.ValidateCustomer(request);

            //if (errors.Any())
            //{
            //    return BadRequest(new
            //    {
            //        status = "Failure",
            //        message = "Validation failed",
            //        validationErrors = ModelState
            //    });
            //}
            var log = new LogCustomerIngestion
            {
                LogId = Guid.NewGuid(),
                RequestTime = DateTime.UtcNow,
                RawPayload = JsonSerializer.Serialize(request),
                HttpStatus = 202,
                Status = Enums.IngestionStatus.SUCCESS,
                ProcessStatus = Enums.ProcessStatus.PENDING
            };

            _context.LogCustomerIngestions.Add(log);
            await _context.SaveChangesAsync();

            return Accepted(new
            {
                status = "Success",
                message = "Customer data received and queued for processing.",
                referenceId = log.LogId
            });
        }
    }
}
