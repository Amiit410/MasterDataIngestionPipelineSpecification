namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class IngestResponse
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? ReferenceId { get; set; }
        public List<ValidationError>? ValidationErrors { get; set; }
    }
}
