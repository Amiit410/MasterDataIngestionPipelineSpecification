namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class ValidationError
    {
        public string Field { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}
