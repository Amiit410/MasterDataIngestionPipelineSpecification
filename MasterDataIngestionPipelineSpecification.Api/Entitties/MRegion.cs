namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class MRegion
    {
        public int RegionId { get; set; }
        public string RegionCode { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public ICollection<MCity> Cities { get; set; } = new List<MCity>();
    }
}
