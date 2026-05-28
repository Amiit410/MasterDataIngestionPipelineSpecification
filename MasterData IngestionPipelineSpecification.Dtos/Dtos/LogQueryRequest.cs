namespace MasterDataIngestionPipelineSpecification.Api.Dtos
{
    public class LogQueryRequest
    {
        public string Domain { get; set; } = "Customer"; // Customer | Item
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
