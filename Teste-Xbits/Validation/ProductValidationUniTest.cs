using FluentValidation.TestHelper;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.EntitiesValidation;

namespace Teste_Xbits.Validation;

public class ProductValidationUniTest
{
    private readonly ProductValidation _validator = new();

    [Fact]
    [Trait("Validation", "Valid Product")]
    public void Validate_ProductIsValid_NotReturnValidationErrors()
    {
        var validProduct = new Product
        {
            Name = "Valid Product Name",
            Description = "Valid product description",
            Price = 100.50m,
            Code = "PROD001",
            HasExpirationDate = false,
            ExpirationDate = null,
            ProductCategoryId = 1
        };
        
        var result = _validator.TestValidate(validProduct);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "Null Name")]
    public void Validate_NameIsNull_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = null!,
            Description = "Valid description",
            Price = 100.50m,
            Code = "PROD001"
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Empty Name")]
    public void Validate_NameIsEmpty_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "",
            Description = "Valid description",
            Price = 100.50m,
            Code = "PROD001"
        };
        
        var result = _validator.TestValidate(invalidProduct);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Name Too Long")]
    public void Validate_NameIsTooLong_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = new string('a', 201),
            Description = "Valid description",
            Price = 100.50m,
            Code = "PROD001"
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Description Too Long")]
    public void Validate_DescriptionIsTooLong_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = new string('a', 1001),
            Price = 100.50m,
            Code = "PROD001"
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    [Trait("Validation", "Price Zero")]
    public void Validate_PriceIsZero_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 0m,
            Code = "PROD001"
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    [Trait("Validation", "Price Negative")]
    public void Validate_PriceIsNegative_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = -10m,
            Code = "PROD001"
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    [Trait("Validation", "Price Too High")]
    public void Validate_PriceIsTooHigh_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 1000000m,
            Code = "PROD001"
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    [Trait("Validation", "Null Code")]
    public void Validate_CodeIsNull_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 100.50m,
            Code = null!
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    [Trait("Validation", "Empty Code")]
    public void Validate_CodeIsEmpty_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 100.50m,
            Code = ""
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    [Trait("Validation", "Code Too Long")]
    public void Validate_CodeIsTooLong_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 100.50m,
            Code = new string('a', 51)
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Code);
    }

    [Fact]
    [Trait("Validation", "Expiration Date Required When HasExpirationDate Is True")]
    public void Validate_HasExpirationDateIsTrueAndExpirationDateIsNull_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 100.50m,
            Code = "PROD001",
            HasExpirationDate = true,
            ExpirationDate = null
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.ExpirationDate);
    }

    [Fact]
    [Trait("Validation", "Expiration Date Not Required When HasExpirationDate Is False")]
    public void Validate_HasExpirationDateIsFalseAndExpirationDateIsNull_ReturnHaveValidationError()
    {
        var validProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 100.50m,
            Code = "PROD001",
            HasExpirationDate = false,
            ExpirationDate = null
        };
        
        var result = _validator.TestValidate(validProduct);
        
        result.ShouldNotHaveValidationErrorFor(x => x.ExpirationDate);
    }

    [Fact]
    [Trait("Validation", "Invalid ProductCategoryId")]
    public void Validate_ProductCategoryIdIsZero_ReturnValidationError()
    {
        var invalidProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 100.50m,
            Code = "PROD001",
            ProductCategoryId = 0
        };
        
        var result = _validator.TestValidate(invalidProduct);

        result.ShouldHaveValidationErrorFor(x => x.ProductCategoryId);
    }

    [Fact]
    [Trait("Validation", "Null ProductCategoryId Is Allowed")]
    public void Validate_ProductCategoryIdIsNull_ReturnValidationError()
    {
        var validProduct = new Product
        {
            Name = "Valid Name",
            Description = "Valid description",
            Price = 100.50m,
            Code = "PROD001",
            ProductCategoryId = 1
        };
        
        var result = _validator.TestValidate(validProduct);
        
        result.ShouldNotHaveValidationErrorFor(x => x.ProductCategoryId);
    }

    [Fact]
    [Trait("Validation", "Boundary Values")]
    public void Validate_FieldsAtBoundaryValues_ReturnValidationErrors()
    {
        var validProduct = new Product
        {
            Name = new string('a', 200),
            Description = new string('a', 1000),
            Price = 999999.99m,
            Code = new string('a', 50),
            HasExpirationDate = true,
            ExpirationDate = DateTime.Now.AddDays(30),
            ProductCategoryId = 1
        };
        
        var result = _validator.TestValidate(validProduct);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "All Fields Invalid")]
    public void Validate_AllFieldsAreInvalid_SReturnMultipleValidationErrors()
    {
        var invalidProduct = new Product
        {
            Name = "",
            Description = new string('a', 1001),
            Price = -10m,
            Code = "",
            HasExpirationDate = true,
            ExpirationDate = null,
            ProductCategoryId = 0
        };
        
        var result = _validator.TestValidate(invalidProduct);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
        result.ShouldHaveValidationErrorFor(x => x.Price);
        result.ShouldHaveValidationErrorFor(x => x.Code);
        result.ShouldHaveValidationErrorFor(x => x.ExpirationDate);
        result.ShouldHaveValidationErrorFor(x => x.ProductCategoryId);
    }
}