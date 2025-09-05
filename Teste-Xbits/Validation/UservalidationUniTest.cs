using FluentValidation.TestHelper;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.EntitiesValidation;

namespace Teste_Xbits.Validation;

public class UservalidationUniTest
{
    private readonly UserValidation _validator = new();

    [Fact]
    [Trait("Validation", "Valid User")]
    public void Validate_UserIsValid_NotReturnValidationErrors()
    {
        var validUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(validUser);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "Null Name")]
    public void Validate_NameIsNull_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = null!,
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Empty Name")]
    public void Validate_NameIsEmpty_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "",
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Name Too Short")]
    public void Validate_NameIsTooShort_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "Jo",
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Name Too Long")]
    public void Validate_NameIsTooLong_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = new string('a', 101),
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    [Trait("Validation", "Null Email")]
    public void Validate_EmailIsNull_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = null!,
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Validation", "Empty Email")]
    public void Validate_EmailIsEmpty_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Validation", "Invalid Email Format")]
    public void Validate_EmailIsInvalid_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "invalid-email",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Validation", "Email Too Long")]
    public void Validate_EmailIsTooLong_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = new string('a', 151) + "@email.com",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    [Trait("Validation", "Null CPF")]
    public void Validate_CpfIsNull_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = null!,
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    [Trait("Validation", "Empty CPF")]
    public void Validate_CpfIsEmpty_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);

        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    [Trait("Validation", "CPF Too Short")]
    public void Validate_CpfIsTooShort_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "1234567890",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    [Trait("Validation", "CPF Too Long")]
    public void Validate_CpfIsTooLong_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "123456789012",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Fact]
    [Trait("Validation", "Null Password")]
    public void Validate_PasswordIsNull_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = null!,
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.PasswordHash);
    }

    [Fact]
    [Trait("Validation", "Empty Password")]
    public void Validate_PasswordIsEmpty_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.PasswordHash);
    }

    [Fact]
    [Trait("Validation", "Password Too Short")]
    public void Validate_PasswordIsTooShort_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "pass",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.PasswordHash);
    }

    [Fact]
    [Trait("Validation", "Privacy Policy Not Accepted")]
    public void Validate_PrivacyPolicyNotAccepted_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = false,
            AcceptTermsOfUse = true
        };

        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.AcceptPrivacyPolicy);
    }

    [Fact]
    [Trait("Validation", "Terms Of Use Not Accepted")]
    public void Validate_TermsOfUseNotAccepted_ReturnValidationError()
    {
        var invalidUser = new User
        {
            Name = "João Silva",
            Email = "joao.silva@email.com",
            Cpf = "12345678901",
            PasswordHash = "password123",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = false
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.AcceptTermsOfUse);
    }

    [Fact]
    [Trait("Validation", "Boundary Values")]
    public void Validate_FieldsAtBoundaryValues_NotReturnValidationErrors()
    {
        var validUser = new User
        {
            Name = new string('a', 100),
            Email = new string('a', 140) + "@email.com",
            Cpf = new string('1', 11),
            PasswordHash = new string('a', 6),
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(validUser);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "Minimum Length Values")]
    public void Validate_FieldsAtMinimumLength_NotReturnValidationErrors()
    {
        var validUser = new User
        {
            Name = "Ana",
            Email = "a@b.com",
            Cpf = "12345678901", 
            PasswordHash = "123456",
            AcceptPrivacyPolicy = true,
            AcceptTermsOfUse = true
        };
        
        var result = _validator.TestValidate(validUser);
        
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    [Trait("Validation", "All Fields Invalid")]
    public void Validate_AllFieldsAreInvalid_ReturnMultipleValidationErrors()
    {
        var invalidUser = new User
        {
            Name = "A",
            Email = "invalid", 
            Cpf = "123", 
            PasswordHash = "123", 
            AcceptPrivacyPolicy = false,
            AcceptTermsOfUse = false
        };
        
        var result = _validator.TestValidate(invalidUser);
        
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Cpf);
        result.ShouldHaveValidationErrorFor(x => x.PasswordHash);
        result.ShouldHaveValidationErrorFor(x => x.AcceptPrivacyPolicy);
        result.ShouldHaveValidationErrorFor(x => x.AcceptTermsOfUse);
    }
}