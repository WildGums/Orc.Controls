namespace Orc.Controls.Automation.Converters
{
    using System.Collections.Generic;
    using Catel.Data;
    using Orc.Automation;

    public class SerializableValidationContext
    {
        public List<SerializableBusinessRuleValidationResult> BusinessRuleResults { get; set; }
        public List<SerializableFieldValidationResult> FieldRuleResults { get; set; }
    }

    public class SerializableBusinessRuleValidationResult
    {
        public object Tag { get; set; }
        public string Message { get; set; }
        public ValidationResultType ValidationType { get; set; }
    }

    public class SerializableFieldValidationResult
    {
        public object Tag { get; set; }
        public string PropertyName { get; set; }
        public string Message { get; set; }
        public ValidationResultType ValidationType { get; set; }
    }

    public class FieldValidationResultSerializableConverter : SerializationValueConverterBase<FieldValidationResult, SerializableFieldValidationResult>
    {
        public override object ConvertFrom(FieldValidationResult value)
        {
            var serializableValue = new SerializableFieldValidationResult
            {
                Tag = value.Tag,
                PropertyName = value.PropertyName,
                ValidationType = value.ValidationResultType,
                Message = value.Message
            };

            return serializableValue;
        }

        public override object ConvertTo(SerializableFieldValidationResult value)
        {
            return new FieldValidationResult(value.PropertyName, value.ValidationType, value.Message)
            {
                Tag = value.Tag
            };
        }
    }

    public class BusinessRuleValidationResultSerializableConverter : SerializationValueConverterBase<BusinessRuleValidationResult, SerializableBusinessRuleValidationResult>
    {
        public override object ConvertFrom(BusinessRuleValidationResult value)
        {
            var serializableValue = new SerializableBusinessRuleValidationResult
            {
                Tag = value.Tag, 
                Message = value.Message,
                ValidationType = value.ValidationResultType
            };

            return serializableValue;
        }

        public override object ConvertTo(SerializableBusinessRuleValidationResult value)
        {
            return new BusinessRuleValidationResult(value.ValidationType, value.Message)
            {
                Tag = value.Tag
            };
        }
    }

    public class ValidationContextSerializableConverter : SerializationValueConverterBase<ValidationContext, SerializableValidationContext>
    {
        private readonly BusinessRuleValidationResultSerializableConverter _businessRuleValidationResultConverter = new ();
        private readonly FieldValidationResultSerializableConverter _fieldRuleValidationResultConverter = new ();

        public override object ConvertFrom(ValidationContext value)
        {
            var validations = value.GetValidations();

            var businessRuleResults = new List<SerializableBusinessRuleValidationResult>();
            var fieldRuleResults = new List<SerializableFieldValidationResult>();

            foreach (var validationResult in validations)
            {
                if (validationResult is BusinessRuleValidationResult businessValidationResult)
                {
                    var serializableBusinessRule = _businessRuleValidationResultConverter.ConvertFrom(businessValidationResult) as SerializableBusinessRuleValidationResult;
                    businessRuleResults.Add(serializableBusinessRule);

                    continue;
                }

                if (validationResult is FieldValidationResult fieldValidationResult)
                {
                    var serializableFieldRule = _fieldRuleValidationResultConverter.ConvertFrom(fieldValidationResult) as SerializableFieldValidationResult;
                    fieldRuleResults.Add(serializableFieldRule);

                    continue;
                }
            }

            return new SerializableValidationContext
            {
                BusinessRuleResults = businessRuleResults,
                FieldRuleResults = fieldRuleResults
            };
        }

        public override object ConvertTo(SerializableValidationContext value)
        {
            var validationContext = new ValidationContext();

            foreach (var validationResult in value.BusinessRuleResults)
            {
                var businessRuleResult = _businessRuleValidationResultConverter.ConvertTo(validationResult) as IBusinessRuleValidationResult;

                validationContext.Add(businessRuleResult);
            }

            foreach (var validationResult in value.FieldRuleResults)
            {
                var fieldRuleResult = _fieldRuleValidationResultConverter.ConvertTo(validationResult) as IFieldValidationResult;

                validationContext.Add(fieldRuleResult);
            }

            return validationContext;
        }
    }
}
