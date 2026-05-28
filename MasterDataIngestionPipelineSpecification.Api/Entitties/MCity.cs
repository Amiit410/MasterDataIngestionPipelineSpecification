namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class MCity
    {
        public int CityId { get; set; }
        public string CityCode { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public MRegion Region { get; set; } = null!;
    }
}
