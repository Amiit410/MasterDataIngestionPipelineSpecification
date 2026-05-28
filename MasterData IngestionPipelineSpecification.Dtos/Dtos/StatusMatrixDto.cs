namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class StatusMatrixDto
    {
        public int ApiSuccess { get; set; }
        public int ApiFailure { get; set; }
        public int Pending { get; set; }
        public int Processed { get; set; }
        public int Error { get; set; }
    }
}
