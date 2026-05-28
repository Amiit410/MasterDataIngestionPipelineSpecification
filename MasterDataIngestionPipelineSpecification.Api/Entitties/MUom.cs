namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class MUom
    {
        public int UomId { get; set; }
        public string UomCode { get; set; } = string.Empty;
        public ICollection<TItemUomConversion> Conversions { get; set; } = new List<TItemUomConversion>();
    }
}
