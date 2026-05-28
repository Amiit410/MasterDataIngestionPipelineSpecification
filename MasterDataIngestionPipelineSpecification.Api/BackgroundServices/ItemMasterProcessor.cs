using System.Text.Json;
using MasterDataIngestionPipelineSpecification.Api.Data;
using MasterDataIngestionPipelineSpecification.Api.Dtos;
using MasterDataIngestionPipelineSpecification.Api.Entitties;
using MasterDataIngestionPipelineSpecification.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace MasterDataIngestionPipelineSpecification.Api.BackgroundServices
{
    public class ItemMasterProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ItemMasterProcessor> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

        public ItemMasterProcessor(IServiceScopeFactory scopeFactory, ILogger<ItemMasterProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ItemMasterProcessor started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessPendingAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ItemMasterProcessor encountered an error.");
                }
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task ProcessPendingAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var pendingLogs = await db.LogItemIngestions
                .Where(x => x.Status == IngestionStatus.SUCCESS && x.ProcessStatus == ProcessStatus.PENDING)
                .Take(50)
                .ToListAsync(ct);

            if (!pendingLogs.Any()) return;

            _logger.LogInformation("ItemMasterProcessor: Processing {Count} records.", pendingLogs.Count);

            foreach (var log in pendingLogs)
            {
                try
                {
                    var req = JsonSerializer.Deserialize<ItemIngestRequest>(log.RawPayload,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (req?.Material is null) throw new InvalidOperationException("Failed to deserialize payload.");

                    var m = req.Material;

                    // Upsert M_BRAND
                    var brand = await db.MBrands.FirstOrDefaultAsync(b => b.BrandCode == m.BrandCode, ct);
                    if (brand is null)
                    {
                        brand = new MBrand { BrandCode = m.BrandCode!, BrandName = m.BrandName! };
                        db.MBrands.Add(brand);
                        await db.SaveChangesAsync(ct);
                    }
                    else
                    {
                        brand.BrandName = m.BrandName!;
                        await db.SaveChangesAsync(ct);
                    }

                    // Upsert M_CATEGORY
                    var category = await db.MCategories.FirstOrDefaultAsync(c => c.CategoryCode == m.CategoryCode, ct);
                    if (category is null)
                    {
                        category = new MCategory { CategoryCode = m.CategoryCode!, CategoryName = m.CategoryName! };
                        db.MCategories.Add(category);
                        await db.SaveChangesAsync(ct);
                    }
                    else
                    {
                        category.CategoryName = m.CategoryName!;
                        await db.SaveChangesAsync(ct);
                    }

                    // Upsert UOMs into M_UOM
                    var uomEntities = new Dictionary<string, MUom>();
                    foreach (var uomDto in m.UomList ?? new())
                    {
                        if (string.IsNullOrWhiteSpace(uomDto.Uom)) continue;
                        var uomCode = uomDto.Uom.ToUpper();
                        var uom = await db.MUoms.FirstOrDefaultAsync(u => u.UomCode == uomCode, ct);
                        if (uom is null)
                        {
                            uom = new MUom { UomCode = uomCode };
                            db.MUoms.Add(uom);
                            await db.SaveChangesAsync(ct);
                        }
                        uomEntities[uomCode] = uom;
                    }

                    // Upsert T_ITEM
                    var item = await db.TItems
                        .Include(i => i.UomConversions)
                        .FirstOrDefaultAsync(i => i.ItemCode == m.ItemCode, ct);

                    if (item is null)
                    {
                        item = new TItem
                        {
                            ItemCode = m.ItemCode!,
                            ItemName = m.ItemName!,
                            SalesOrgCode = m.SalesOrgCode!,
                            BaseUOM = m.BaseUOM!,
                            IsBatchEnabled = m.IsBatchEnabled!.Value,
                            IsActive = m.IsActive!,
                            BrandId = brand.BrandId,
                            CategoryId = category.CategoryId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        db.TItems.Add(item);
                        await db.SaveChangesAsync(ct);
                    }
                    else
                    {
                        item.ItemName = m.ItemName!;
                        item.SalesOrgCode = m.SalesOrgCode!;
                        item.BaseUOM = m.BaseUOM!;
                        item.IsBatchEnabled = m.IsBatchEnabled!.Value;
                        item.IsActive = m.IsActive!;
                        item.BrandId = brand.BrandId;
                        item.CategoryId = category.CategoryId;
                        item.UpdatedAt = DateTime.UtcNow;
                        await db.SaveChangesAsync(ct);
                    }

                    // Upsert T_ITEM_UOM_CONVERSION
                    foreach (var uomDto in m.UomList ?? new())
                    {
                        if (string.IsNullOrWhiteSpace(uomDto.Uom) || !uomDto.ConversionFactor.HasValue) continue;
                        var uomCode = uomDto.Uom.ToUpper();
                        if (!uomEntities.TryGetValue(uomCode, out var uomEntity)) continue;

                        var conv = await db.TItemUomConversions
                            .FirstOrDefaultAsync(c => c.ItemId == item.ItemId && c.UomId == uomEntity.UomId, ct);

                        if (conv is null)
                        {
                            db.TItemUomConversions.Add(new TItemUomConversion
                            {
                                ItemId = item.ItemId,
                                UomId = uomEntity.UomId,
                                ConversionFactor = uomDto.ConversionFactor.Value
                            });
                        }
                        else
                        {
                            conv.ConversionFactor = uomDto.ConversionFactor.Value;
                        }
                    }
                    await db.SaveChangesAsync(ct);

                    log.ProcessStatus = ProcessStatus.PROCESSED;
                    log.ProcessedAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process item log {LogId}.", log.LogId);
                    log.ProcessStatus = ProcessStatus.ERROR;
                    log.ProcessError = ex.Message;
                    log.ProcessedAt = DateTime.UtcNow;
                }

                await db.SaveChangesAsync(ct);
            }
        }
    }
}
