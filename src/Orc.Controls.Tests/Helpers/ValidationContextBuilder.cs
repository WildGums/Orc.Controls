namespace Orc.Controls.Tests;

using Catel.Data;

public class ValidationContextBuilder
{
    public static ValidationContextBuilder Start() => new ();

    private readonly ValidationContext _validationContext = new();

    private string _currentTag;
    private ValidationResultType _currentValidationResultType;

    public ValidationContext Result()
    {
        return _validationContext;
    }

    public ValidationContextBuilder Tag(string tag)
    {
        _currentTag = tag;

        return this;
    }

    public ValidationContextBuilder Warnings()
    {
        return Type(ValidationResultType.Warning);
    }

    public ValidationContextBuilder Errors()
    {
        return Type(ValidationResultType.Error);
    }

    public ValidationContextBuilder Business(string message)
    {
        _validationContext.Add(
            new BusinessRuleValidationResult(_currentValidationResultType, message) { Tag = _currentTag });

        return this;
    }

    public ValidationContextBuilder Field(string property, string message)
    {
        _validationContext.Add(
            new FieldValidationResult(property, _currentValidationResultType, message) { Tag = _currentTag });

        return this;
    }

    private ValidationContextBuilder Type(ValidationResultType validationResultType)
    {
        _currentValidationResultType = validationResultType;

        return this;
    }
}
