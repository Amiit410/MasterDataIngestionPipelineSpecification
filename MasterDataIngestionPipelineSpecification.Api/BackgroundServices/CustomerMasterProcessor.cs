using System.Text.Json;
using MasterDataIngestionPipelineSpecification.Api.Data;
using MasterDataIngestionPipelineSpecification.Api.Dtos;
using MasterDataIngestionPipelineSpecification.Api.Entitties;
using MasterDataIngestionPipelineSpecification.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace MasterDataIngestionPipelineSpecification.Api.BackgroundServices
{
    public class CustomerMasterProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CustomerMasterProcessor> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

        public CustomerMasterProcessor(IServiceScopeFactory scopeFactory, ILogger<CustomerMasterProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CustomerMasterProcessor started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessPendingAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CustomerMasterProcessor encountered an error.");
                }
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task ProcessPendingAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var pendingLogs = await db.LogCustomerIngestions
                .Where(x => x.Status == IngestionStatus.SUCCESS && x.ProcessStatus == ProcessStatus.PENDING)
                .Take(50)
                .ToListAsync(ct);

            if (!pendingLogs.Any()) return;

            _logger.LogInformation("CustomerMasterProcessor: Processing {Count} records.", pendingLogs.Count);

            foreach (var log in pendingLogs)
            {
                try
                {
                    var req = JsonSerializer.Deserialize<CustomerIngestRequest>(log.RawPayload,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (req is null) throw new InvalidOperationException("Failed to deserialize payload.");

                    // Upsert M_REGION
                    var region = await db.MRegions.FirstOrDefaultAsync(r => r.RegionCode == req.RegionCode, ct);
                    if (region is null)
                    {
                        region = new MRegion { RegionCode = req.RegionCode!, RegionName = req.RegionName! };
                        db.MRegions.Add(region);
                        await db.SaveChangesAsync(ct);
                    }
                    else
                    {
                        region.RegionName = req.RegionName!;
                        await db.SaveChangesAsync(ct);
                    }

                    // Upsert M_CITY
                    var city = await db.MCities.FirstOrDefaultAsync(c => c.CityCode == req.CityCode, ct);
                    if (city is null)
                    {
                        city = new MCity { CityCode = req.CityCode!, CityName = req.CityName!, RegionId = region.RegionId };
                        db.MCities.Add(city);
                        await db.SaveChangesAsync(ct);
                    }
                    else
                    {
                        city.CityName = req.CityName!;
                        city.RegionId = region.RegionId;
                        await db.SaveChangesAsync(ct);
                    }

                    // Upsert M_CHANNEL (if provided)
                    if (!string.IsNullOrWhiteSpace(req.ChannelCode))
                    {
                        var channel = await db.MChannels.FirstOrDefaultAsync(c => c.ChannelCode == req.ChannelCode, ct);
                        if (channel is null)
                            db.MChannels.Add(new MChannel { ChannelCode = req.ChannelCode, ChannelName = req.ChannelName ?? req.ChannelCode });
                        else
                            channel.ChannelName = req.ChannelName ?? channel.ChannelName;
                        await db.SaveChangesAsync(ct);
                    }

                    // Upsert M_PAYMENT_TERM (if provided)
                    if (!string.IsNullOrWhiteSpace(req.PaymentTermCode))
                    {
                        var term = await db.MPaymentTerms.FirstOrDefaultAsync(t => t.TermCode == req.PaymentTermCode, ct);
                        if (term is null)
                            db.MPaymentTerms.Add(new MPaymentTerm
                            {
                                TermCode = req.PaymentTermCode,
                                TermName = req.PaymentTermName ?? req.PaymentTermCode,
                                CreditDays = req.CreditDays ?? 0,
                                CreditLimit = req.CreditLimit ?? 0
                            });
                        else
                        {
                            term.TermName = req.PaymentTermName ?? term.TermName;
                            term.CreditDays = req.CreditDays ?? term.CreditDays;
                            term.CreditLimit = req.CreditLimit ?? term.CreditLimit;
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Upsert T_CUSTOMER
                    var customer = await db.TCustomers.FirstOrDefaultAsync(c => c.CustomerCode == req.CustomerCode, ct);
                    if (customer is null)
                    {
                        customer = new TCustomer
                        {
                            CustomerCode = req.CustomerCode!,
                            CustomerName = req.CustomerName!,
                            ContactNo = req.ContactNo!,
                            Email = req.Email!,
                            CustomerType = req.CustomerType!,
                            IsActive = req.IsActive!.Value,
                            IsBlocked = req.IsBlocked!.Value,
                            CreditLimit = req.CreditLimit ?? 0,
                            CreditDays = req.CreditDays ?? 0,
                            CityId = city.CityId,
                            RegionId = region.RegionId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        db.TCustomers.Add(customer);
                    }
                    else
                    {
                        customer.CustomerName = req.CustomerName!;
                        customer.ContactNo = req.ContactNo!;
                        customer.Email = req.Email!;
                        customer.CustomerType = req.CustomerType!;
                        customer.IsActive = req.IsActive!.Value;
                        customer.IsBlocked = req.IsBlocked!.Value;
                        customer.CreditLimit = req.CreditLimit ?? customer.CreditLimit;
                        customer.CreditDays = req.CreditDays ?? customer.CreditDays;
                        customer.CityId = city.CityId;
                        customer.RegionId = region.RegionId;
                        customer.UpdatedAt = DateTime.UtcNow;
                    }
                    await db.SaveChangesAsync(ct);

                    log.ProcessStatus = ProcessStatus.PROCESSED;
                    log.ProcessedAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process customer log {LogId}.", log.LogId);
                    log.ProcessStatus = ProcessStatus.ERROR;
                    log.ProcessError = ex.Message;
                    log.ProcessedAt = DateTime.UtcNow;
                }

                await db.SaveChangesAsync(ct);
            }
        }
    }
}
