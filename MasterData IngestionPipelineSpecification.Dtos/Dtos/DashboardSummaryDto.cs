namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class DashboardSummaryDto
    {
        public int TotalRequests { get; set; }
        public DomainMetrics CustomerMetrics { get; set; } = new();
        public DomainMetrics ItemMetrics { get; set; } = new();
        public StatusMatrixDto IngestionMatrix { get; set; } = new();
        public StatusMatrixDto ProcessingMatrix { get; set; } = new();
    }
}
