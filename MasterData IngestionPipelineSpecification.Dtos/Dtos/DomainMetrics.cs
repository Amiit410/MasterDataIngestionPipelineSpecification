namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class DomainMetrics
    {
        public string Domain { get; set; } = string.Empty;
        public int TotalRequests { get; set; }
        public double ApiSuccessRate { get; set; }
        public double ProcessingSuccessRate { get; set; }   
    }
}
