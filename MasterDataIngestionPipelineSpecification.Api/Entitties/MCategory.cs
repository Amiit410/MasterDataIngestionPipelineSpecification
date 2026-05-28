using System.Timers;

namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class MCategory
    {
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<TItem> Items { get; set; } = new List<TItem>();
    }
}
