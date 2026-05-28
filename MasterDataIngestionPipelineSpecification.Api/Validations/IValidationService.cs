using MasterDataIngestionPipelineSpecification.Api.Dtos;

namespace MasterDataIngestionPipelineSpecification.Api.Validations
{
    public interface IValidationService
    {
        List<ValidationError> ValidateCustomer(CustomerIngestRequest request);
        List<ValidationError> ValidateItem(ItemIngestRequest request);
    }
}
