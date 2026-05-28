using System.Text.RegularExpressions;
using MasterDataIngestionPipelineSpecification.Api.Dtos;

namespace MasterDataIngestionPipelineSpecification.Api.Validations
{
    public class ValidationService : IValidationService
    {
        private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public List<ValidationError> ValidateCustomer(CustomerIngestRequest req)
        {
            var errors = new List<ValidationError>();

            CheckRequired(errors, req.CustomerCode, nameof(req.CustomerCode));
            CheckRequired(errors, req.CustomerName, nameof(req.CustomerName));
            CheckRequired(errors, req.ContactNo, nameof(req.ContactNo));
            CheckRequired(errors, req.CityCode, nameof(req.CityCode));
            CheckRequired(errors, req.CityName, nameof(req.CityName));
            CheckRequired(errors, req.RegionCode, nameof(req.RegionCode));
            CheckRequired(errors, req.RegionName, nameof(req.RegionName));
            CheckRequired(errors, req.CustomerType, nameof(req.CustomerType));

            if (string.IsNullOrWhiteSpace(req.Email))
                errors.Add(new ValidationError { Field = nameof(req.Email), Reason = "Email is required." });
            else if (!EmailRegex.IsMatch(req.Email))
                errors.Add(new ValidationError { Field = nameof(req.Email), Reason = "Email must be a valid email address." });

            if (req.IsActive is null)
                errors.Add(new ValidationError { Field = nameof(req.IsActive), Reason = "IsActive is required and must be a boolean." });

            if (req.IsBlocked is null)
                errors.Add(new ValidationError { Field = nameof(req.IsBlocked), Reason = "IsBlocked is required and must be a boolean." });

            if (req.CreditLimit is null)
                errors.Add(new ValidationError { Field = nameof(req.CreditLimit), Reason = "CreditLimit is required and must be numeric." });

            if (req.CreditDays is null)
                errors.Add(new ValidationError { Field = nameof(req.CreditDays), Reason = "CreditDays is required and must be numeric." });

            // Business rule: non-CASH customers must have creditLimit > 0 and creditDays > 0
            if (!string.IsNullOrWhiteSpace(req.CustomerType) &&
                !req.CustomerType.Equals("CASH", StringComparison.OrdinalIgnoreCase))
            {
                if (req.CreditLimit.HasValue && req.CreditLimit.Value <= 0)
                    errors.Add(new ValidationError { Field = nameof(req.CreditLimit), Reason = "CreditLimit must be greater than 0 for non-CASH customers." });
                if (req.CreditDays.HasValue && req.CreditDays.Value <= 0)
                    errors.Add(new ValidationError { Field = nameof(req.CreditDays), Reason = "CreditDays must be greater than 0 for non-CASH customers." });
            }

            return errors;
        }

        public List<ValidationError> ValidateItem(ItemIngestRequest req)
        {
            var errors = new List<ValidationError>();

            if (req.Material is null)
            {
                errors.Add(new ValidationError { Field = "material", Reason = "The 'material' object is required." });
                return errors;
            }

            var m = req.Material;
            CheckRequired(errors, m.ItemCode, "material.itemCode");
            CheckRequired(errors, m.ItemName, "material.itemName");
            CheckRequired(errors, m.SalesOrgCode, "material.salesOrgCode");
            CheckRequired(errors, m.BaseUOM, "material.baseUOM");
            CheckRequired(errors, m.BrandCode, "material.brandCode");
            CheckRequired(errors, m.BrandName, "material.brandName");
            CheckRequired(errors, m.CategoryCode, "material.categoryCode");
            CheckRequired(errors, m.CategoryName, "material.categoryName");

            if (m.IsBatchEnabled is null)
                errors.Add(new ValidationError { Field = "material.isBatchEnabled", Reason = "isBatchEnabled is required and must be a boolean." });

            if (string.IsNullOrWhiteSpace(m.IsActive))
                errors.Add(new ValidationError { Field = "material.isActive", Reason = "isActive is required." });
            else if (m.IsActive != "0" && m.IsActive != "1")
                errors.Add(new ValidationError { Field = "material.isActive", Reason = "isActive must be '0' or '1'." });

            if (m.UomList is null || m.UomList.Count == 0)
            {
                errors.Add(new ValidationError { Field = "material.uomList", Reason = "uomList is required and must be a non-empty array." });
            }
            else
            {
                for (int i = 0; i < m.UomList.Count; i++)
                {
                    var uom = m.UomList[i];
                    if (string.IsNullOrWhiteSpace(uom.Uom))
                        errors.Add(new ValidationError { Field = $"material.uomList[{i}].uom", Reason = "uom is required." });
                    if (uom.ConversionFactor is null)
                        errors.Add(new ValidationError { Field = $"material.uomList[{i}].conversionFactor", Reason = "conversionFactor is required and must be numeric." });
                    else if (uom.ConversionFactor.Value <= 0)
                        errors.Add(new ValidationError { Field = $"material.uomList[{i}].conversionFactor", Reason = "conversionFactor must be greater than 0." });
                }
            }

            return errors;
        }

        private static void CheckRequired(List<ValidationError> errors, string? value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                errors.Add(new ValidationError { Field = fieldName, Reason = $"{fieldName} is required and must be a non-empty string." });
        }
    }
}
