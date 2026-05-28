namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class TItemUomConversion
    {
        public int ConversionId { get; set; }
        public int ItemId { get; set; }
        public TItem Item { get; set; } = null!;
        public int UomId { get; set; }
        public MUom Uom { get; set; } = null!;
        public decimal ConversionFactor { get; set; }
    }
}
