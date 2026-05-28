namespace MasterDataIngestionPipelineSpecification.Api.Entitties
{
    public class MPaymentTerm
    {
        public int PaymentTermId { get; set; }
        public string TermCode { get; set; } = string.Empty;
        public string TermName { get; set; } = string.Empty;
        public int CreditDays { get; set; }
        public decimal CreditLimit { get; set; }
    }
}
