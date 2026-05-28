namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class TCustomer
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public decimal CreditLimit { get; set; }
        public int CreditDays { get; set; }
        public int CityId { get; set; }
        public MCity City { get; set; } = null!;
        public int RegionId { get; set; }
        public MRegion Region { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
