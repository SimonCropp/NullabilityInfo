using System.Threading.Tasks;
using FluentValidation;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class FluentValidationTests
{
    [Fact]
    public Task Nulls_NoValues()
    {
        var validator = new TargetWithNullsValidator();

        var result = validator.Validate(new TargetWithNulls());
        return Verifier.Verify(result);
    }

    [Fact]
    public Task Nulls_WithValues()
    {
        var validator = new TargetWithNullsValidator();

        var target = new TargetWithNulls
        {
            ReadWrite = "a", Write = "a"
        };
        var result = validator.Validate(target);
        return Verifier.Verify(result);
    }

    class TargetWithNullsValidator : 
        RespectNullValidator<TargetWithNulls>
    {
    }

    class TargetWithNulls
    {
        string? write;
        public string? ReadWrite { get; set; }
        public string? Read { get; }

        public string? Write
        {
            set => write = value;
        }
    }

    [Fact]
    public Task NoNulls_NoValues()
    {
        var validator = new TargetWithNoNullsValidator();

        var result = validator.Validate(new TargetWithNoNulls());
        return Verifier.Verify(result);
    }

    [Fact]
    public Task NoNulls_WithValues()
    {
        var validator = new TargetWithNoNullsValidator();

        var target = new TargetWithNoNulls
        {
            ReadWrite = "a",
            Write = "a"
        };
        var result = validator.Validate(target);
        return Verifier.Verify(result);
    }

    class TargetWithNoNullsValidator :
        RespectNullValidator<TargetWithNoNulls>
    {
    }

    class TargetWithNoNulls
    {
        string write;
        public string ReadWrite { get; set; }
        public string Read { get; }

        public string Write
        {
            set => write = value;
        }
    }

    [Fact]
    public Task ValueTypes_NoValues()
    {
        var validator = new TargetValueTypesValidator();

        var result = validator.Validate(new TargetValueTypes());
        return Verifier.Verify(result);
    }

    [Fact]
    public Task ValueTypes_WithValues()
    {
        var validator = new TargetValueTypesValidator();

        var target = new TargetValueTypes
        {
            NotNullable = true,
            Nullable = true,
        };
        var result = validator.Validate(target);
        return Verifier.Verify(result);
    }

    class TargetValueTypesValidator :
        RespectNullValidator<TargetValueTypes>
    {
    }

    class TargetValueTypes
    {
        public bool? Nullable { get; set; }
        public bool NotNullable { get; set; }
    }

    
    [Fact]
    public Task Usage()
    {
        var validator = new PersonValidatorFromBase();

        var target = new Person
        {
            GivenName = "Joe"
        };
        var result = validator.Validate(target);
        return Verifier.Verify(result);
    }

    public class Person
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
    }

    #region RespectNullValidatorUsage

    
    class PersonValidatorFromBase :
        RespectNullValidator<Person>
    {
    }

    #endregion
    #region AddNullableRulesUsage
    class PersonValidatorNonBase :
        AbstractValidator<Person>
    {
        public PersonValidatorNonBase()
        {
            this.AddNullableRules();
        }
    }
    #endregion
    #region Equivalent
    class PersonValidatorEquivalent :
        AbstractValidator<Person>
    {
        public PersonValidatorEquivalent()
        {
            RuleFor(x => x.GivenName).NotNull();
            RuleFor(x => x.FamilyName).NotNull();
        }
    }
    #endregion
}