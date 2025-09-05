using FluentValidation.TestHelper;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.EntitiesValidation;

namespace Teste_Xbits.Validation;

public class LoginValidationUnitTest
{
    private readonly LoginValidation _validator = new();

    [Fact]
    [Trait("Validation", "Valid Login")]
    public void Validate_LoginIsValid_NotReturnValidationErrors()
    {
        var validLogin = new Login
        {
            UserName = "validuser",
            Password = "validpassword123"
        };
        
        var result = _validator.TestValidate(validLogin);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "Null UserName")]
    public void Validate_UserNameIsNull_ReturnValidationError()
    {
        var invalidLogin = new Login
        {
            UserName = null!,
            Password = "validpassword123"
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    [Trait("Validation", "Empty UserName")]
    public void Validate_UserNameIsEmpty_ReturnValidationError()
    {
        var invalidLogin = new Login
        {
            UserName = "",
            Password = "validpassword123"
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    [Trait("Validation", "UserName Too Long")]
    public void Validate_UserNameExceedsMaxLength_ReturnValidationError()
    {
        var invalidLogin = new Login
        {
            UserName = new string('a', 101),
            Password = "validpassword123"
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    [Trait("Validation", "Null Password")]
    public void Validate_PasswordIsNull_ReturnValidationError()
    {
        var invalidLogin = new Login
        {
            UserName = "validuser",
            Password = null!
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    [Trait("Validation", "Empty Password")]
    public void Validate_PasswordIsEmpty_ReturnValidationError()
    {
        var invalidLogin = new Login
        {
            UserName = "validuser",
            Password = ""
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    [Trait("Validation", "Password Too Long")]
    public void Validate_PasswordExceedsMaxLength_ReturnValidationError()
    {
        var invalidLogin = new Login
        {
            UserName = "validuser",
            Password = new string('a', 21)
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    [Trait("Validation", "Both Fields Invalid")]
    public void Validate_BothUserNameAndPasswordAreInvalid_ReturnValidationErrors()
    {
        var invalidLogin = new Login
        {
            UserName = null!,
            Password = null!
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.UserName);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    [Trait("Validation", "Boundary Values")]
    public void Validate_FieldsAtMaxLength_ReturnValidationErrors()
    {
        var validLogin = new Login
        {
            UserName = new string('a', 100),
            Password = new string('a', 20)
        };
        
        var result = _validator.TestValidate(validLogin);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "Username with spaces")]
    public void Validate_UserNameIsWhiteSpace_ReturnValidationError()
    {
        var invalidLogin = new Login
        {
            UserName = "   ",
            Password = "validpassword123"
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.UserName);
    }

    [Fact]
    [Trait("Validation", "Password with spaces")]
    public void Validate_PasswordIsWhiteSpace_ReturnValidationError()
    {
        var invalidLogin = new Login
        {
            UserName = "validuser",
            Password = "   "
        };
        
        var result = _validator.TestValidate(invalidLogin);
        
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}