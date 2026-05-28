using MasterDataIngestionPipelineSpecification.Api.Data;
using MasterDataIngestionPipelineSpecification.Api.Dtos;
using MasterDataIngestionPipelineSpecification.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace MasterDataIngestionPipelineSpecification.Api.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;

        public DashboardService(AppDbContext db) => _db = db;

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            var customers = await _db.LogCustomerIngestions.ToListAsync();
            var items = await _db.LogItemIngestions.ToListAsync();

            int cTotal = customers.Count;
            int iTotal = items.Count;

            int cSuccess = customers.Count(x => x.Status == IngestionStatus.SUCCESS);
            int iSuccess = items.Count(x => x.Status == IngestionStatus.SUCCESS);

            int cProcessed = customers.Count(x => x.ProcessStatus == ProcessStatus.PROCESSED);
            int iProcessed = items.Count(x => x.ProcessStatus == ProcessStatus.PROCESSED);

            return new DashboardSummaryDto
            {
                TotalRequests = cTotal + iTotal,

                CustomerMetrics = new DomainMetrics
                {
                    Domain = "Customer",
                    TotalRequests = cTotal,
                    ApiSuccessRate = cTotal == 0 ? 0 : Math.Round((double)cSuccess / cTotal * 100, 1),
                    ProcessingSuccessRate = cSuccess == 0 ? 0 : Math.Round((double)cProcessed / cSuccess * 100, 1)
                },

                ItemMetrics = new DomainMetrics
                {
                    Domain = "Item",
                    TotalRequests = iTotal,
                    ApiSuccessRate = iTotal == 0 ? 0 : Math.Round((double)iSuccess / iTotal * 100, 1),
                    ProcessingSuccessRate = iSuccess == 0 ? 0 : Math.Round((double)iProcessed / iSuccess * 100, 1)
                },

                IngestionMatrix = new StatusMatrixDto
                {
                    ApiSuccess = cSuccess + iSuccess,
                    ApiFailure = customers.Count(x => x.Status == IngestionStatus.VALIDATION_FAILED)
                               + items.Count(x => x.Status == IngestionStatus.VALIDATION_FAILED)
                },

                ProcessingMatrix = new StatusMatrixDto
                {
                    Pending = customers.Count(x => x.ProcessStatus == ProcessStatus.PENDING && x.Status == IngestionStatus.SUCCESS)
                            + items.Count(x => x.ProcessStatus == ProcessStatus.PENDING && x.Status == IngestionStatus.SUCCESS),
                    Processed = cProcessed + iProcessed,
                    Error = customers.Count(x => x.ProcessStatus == ProcessStatus.ERROR)
                          + items.Count(x => x.ProcessStatus == ProcessStatus.ERROR)
                }
            };
        }

        public async Task<PagedResult<IngestionLogDto>> GetLogsAsync(LogQueryRequest request)
        {
            if (request.Domain == "Customer")
            {
                var query = _db.LogCustomerIngestions.OrderByDescending(x => x.RequestTime);
                int total = await query.CountAsync();
                var rows = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                return new PagedResult<IngestionLogDto>
                {
                    TotalCount = total,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Items = rows.Select(r => new IngestionLogDto
                    {
                        ReferenceId = $"INGEST-C-{r.RequestTime:yyyyMMdd}-{r.LogId.ToString()[..8].ToUpper()}",
                        Domain = "Customer",
                        ReceivedTime = r.RequestTime,
                        ApiStatus = r.HttpStatus,
                        ValidationFailures = r.ValidationDetails,
                        ProcessingStatus = r.ProcessStatus.ToString(),
                        RawRequest = r.RawPayload
                    }).ToList()
                };
            }
            else
            {
                var query = _db.LogItemIngestions.OrderByDescending(x => x.RequestTime);
                int total = await query.CountAsync();
                var rows = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                return new PagedResult<IngestionLogDto>
                {
                    TotalCount = total,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Items = rows.Select(r => new IngestionLogDto
                    {
                        ReferenceId = $"INGEST-I-{r.RequestTime:yyyyMMdd}-{r.LogId.ToString()[..8].ToUpper()}",
                        Domain = "Item",
                        ReceivedTime = r.RequestTime,
                        ApiStatus = r.HttpStatus,
                        ValidationFailures = r.ValidationDetails,
                        ProcessingStatus = r.ProcessStatus.ToString(),
                        RawRequest = r.RawPayload
                    }).ToList()
                };
            }
        }
    }
}
