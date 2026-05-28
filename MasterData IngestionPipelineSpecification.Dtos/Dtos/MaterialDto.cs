namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class MaterialDto
    {
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? SalesOrgCode { get; set; }
        public string? BaseUOM { get; set; }
        public string? BrandCode { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryCode { get; set; }
        public string? CategoryName { get; set; }
        public bool? IsBatchEnabled { get; set; }
        public string? IsActive { get; set; }
        public List<UomDto>? UomList { get; set; }
    }
}
