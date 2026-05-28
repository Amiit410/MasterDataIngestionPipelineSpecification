namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class CustomerIngestRequest
    {
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }
        public string? ContactNo { get; set; }
        public string? Email { get; set; }
        public string? CityCode { get; set; }
        public string? CityName { get; set; }
        public string? RegionCode { get; set; }
        public string? RegionName { get; set; }
        public string? CustomerType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBlocked { get; set; }
        public decimal? CreditLimit { get; set; }
        public int? CreditDays { get; set; }
        public string? ChannelCode { get; set; }
        public string? ChannelName { get; set; }
        public string? PaymentTermCode { get; set; }
        public string? PaymentTermName { get; set; }
    }
}
