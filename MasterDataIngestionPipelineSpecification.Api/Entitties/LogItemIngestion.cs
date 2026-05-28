using MasterDataIngestionPipelineSpecification.Api.Enums;

namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class LogItemIngestion
    {
        public Guid LogId { get; set; } = Guid.NewGuid();
        public DateTime RequestTime { get; set; } = DateTime.UtcNow;
        public string RawPayload { get; set; } = string.Empty;
        public int HttpStatus { get; set; }
        public IngestionStatus Status { get; set; }
        public string? ValidationDetails { get; set; }
        public ProcessStatus ProcessStatus { get; set; } = ProcessStatus.PENDING;
        public string? ProcessError { get; set; }
        public DateTime? ProcessedAt { get; set; }
    }
}
