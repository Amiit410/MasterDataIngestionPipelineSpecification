namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class TItem
    {
        public int ItemId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string SalesOrgCode { get; set; } = string.Empty;
        public string BaseUOM { get; set; } = string.Empty;
        public bool IsBatchEnabled { get; set; }
        public string IsActive { get; set; } = "1";
        public int BrandId { get; set; }
        public MBrand Brand { get; set; } = null!;
        public int CategoryId { get; set; }
        public MCategory Category { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<TItemUomConversion> UomConversions { get; set; } = new List<TItemUomConversion>();
    }
}
