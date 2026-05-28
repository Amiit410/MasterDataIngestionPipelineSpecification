using System.Timers;

namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class MBrand
    {
        public int BrandId { get; set; }
        public string BrandCode { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public ICollection<TItem> Items { get; set; } = new List<TItem>();
    }
}
