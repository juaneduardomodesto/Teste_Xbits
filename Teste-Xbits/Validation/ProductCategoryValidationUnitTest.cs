using FluentValidation.TestHelper;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.EntitiesValidation;

namespace Teste_Xbits.Validation;

public class ProductCategoryValidationUnitTest
{
    private readonly ProductCategoryValidation _validator = new();

    [Fact]
    [Trait("Validation", "Valid ProductCategory")]
    public void Validate_ProductCategoryIsValid_NotReturnValidationErrors()
    {
        var validProductCategory = new ProductCategory
        {
            Name = "Valid Name",
            Description = "Valid Description",
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(validProductCategory);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "Null Name")]
    public void Validate_NameIsNull_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = null!,
            Description = "Valid Description",
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Empty Name")]
    public void Validate_NameIsEmpty_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "",
            Description = "Valid Description",
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(invalidProductCategory);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Name Too Short")]
    public void Validate_NameIsTooShort_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "A",
            Description = "Valid Description",
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Name Too Long")]
    public void Validate_NameIsTooLong_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = new string('a', 101),
            Description = "Valid Description",
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Null Description")]
    public void Validate_DescriptionIsNull_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "Valid Name",
            Description = null!,
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Validation", "Empty Description")]
    public void Validate_DescriptionIsEmpty_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "Valid Name",
            Description = "",
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Validation", "Description Too Short")]
    public void Validate_DescriptionIsTooShort_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "Valid Name",
            Description = "Too short",
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Validation", "Description Too Long")]
    public void Validate_DescriptionIsTooLong_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "Valid Name",
            Description = new string('a', 501),
            ProductCategoryCode = "CAT001"
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Validation", "Null ProductCategoryCode")]
    public void Validate_ProductCategoryCodeIsNull_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "Valid Name",
            Description = "Valid description with enough characters",
            ProductCategoryCode = null!
        };
        
        var result = _validator.TestValidate(invalidProductCategory);

        result.ShouldHaveValidationErrorFor(x => x.ProductCategoryCode);
    }

    [Fact]
    [Trait("Validation", "Empty ProductCategoryCode")]
    public void Validate_ProductCategoryCodeIsEmpty_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "Valid Name",
            Description = "Valid description with enough characters",
            ProductCategoryCode = ""
        };

        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.ProductCategoryCode);
    }

    [Fact]
    [Trait("Validation", "ProductCategoryCode Too Long")]
    public void Validate_ProductCategoryCodeIsTooLong_ReturnValidationError()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "Valid Name",
            Description = "Valid description with enough characters",
            ProductCategoryCode = new string('a', 21)
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.ProductCategoryCode);
    }

    [Fact]
    [Trait("Validation", "Boundary Values")]
    public void Validate_FieldsAtBoundaryValues_ReturnValidationErrors()
    {
        var validProductCategory = new ProductCategory
        {
            Name = new string('a', 100),
            Description = new string('a', 500),
            ProductCategoryCode = new string('a', 20)
        };
        
        var result = _validator.TestValidate(validProductCategory);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "Minimum Length Values")]
    public void Validate_FieldsAtMinimumLength_ReturnValidationErrors()
    {
        var validProductCategory = new ProductCategory
        {
            Name = "Ab",
            Description = "Exactly 10",
            ProductCategoryCode = "A"
        };
        
        var result = _validator.TestValidate(validProductCategory);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "All Fields Invalid")]
    public void Validate_AllFieldsAreInvalid_ReturnMultipleValidationErrors()
    {
        var invalidProductCategory = new ProductCategory
        {
            Name = "",
            Description = "Short",
            ProductCategoryCode = new string('a', 21)
        };
        
        var result = _validator.TestValidate(invalidProductCategory);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.ProductCategoryCode);
    }
}