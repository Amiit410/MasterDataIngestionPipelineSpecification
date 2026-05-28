namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class IngestionLogDto
    {
        public string ReferenceId { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public DateTime ReceivedTime { get; set; }
        public int ApiStatus { get; set; }
        public string? ValidationFailures { get; set; }
        public string ProcessingStatus { get; set; } = string.Empty;
        public string RawRequest { get; set; } = string.Empty;
    }
}
