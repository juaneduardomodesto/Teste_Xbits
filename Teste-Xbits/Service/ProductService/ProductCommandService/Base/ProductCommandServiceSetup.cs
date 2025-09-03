using Microsoft.Extensions.Configuration;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductRequest;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.Service.ProductService.ProductCommandService.Base;

public class ProductCommandServiceSetup
{
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<Product>> Validator;
    protected readonly Mock<ILoggerHandler> LoggerHandler;
    protected readonly Mock<IProductRepository> ProductRepository;
    protected readonly Mock<IProductCategoryRepository> CategoryRepository;
    protected readonly Mock<IConfiguration> Configuration;
    protected readonly Mock<IProductMapper> ProductMapper;
    protected readonly Dictionary<string, string> Errors;
    protected readonly ValidationResponse ValidationResponse;
    protected readonly ApplicationService.Services.ProductService.ProductCommandService ProductCommandService;

    protected ProductCommandServiceSetup()
    {
        NotificationHandler = new Mock<INotificationHandler>();
        Validator = new Mock<IValidate<Product>>();
        LoggerHandler = new Mock<ILoggerHandler>();
        ProductRepository = new Mock<IProductRepository>();
        CategoryRepository = new Mock<IProductCategoryRepository>();
        ProductMapper = new Mock<IProductMapper>();
        Configuration = new Mock<IConfiguration>();
        
        Errors = [];
        ValidationResponse = ValidationResponse.CreateResponse(Errors);

        ProductCommandService = new ApplicationService.Services.ProductService.ProductCommandService(
            NotificationHandler.Object,
            Validator.Object,
            LoggerHandler.Object,
            ProductMapper.Object,
            ProductRepository.Object,
            CategoryRepository.Object
        );
    }
    
    protected void CreateNotification() => Errors.Add("Error", "Error");
    
    protected Product CreateValidProduct()
    {
        return new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.50m,
            Code = "12345",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
    
    protected Product CreateInvalidProduct()
    {
        return new Product
        {
            Id = 0,
            Name = "",
            Description = "Test Description",
            Price = -10m,
            Code = "12345",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
    
    protected ProductRegisterRequest CreateValidProductCreateRequest() => 
        new()
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.50m,
            Code = "12345",
            ExpirationDate = DateTime.Today.AddDays(1),
            HasExpirationDate = true,
            ProductCategoryId = null
        };
    
    protected ProductRegisterRequest CreateInvalidProductCreateRequest() => 
        new()
        {
            Name = "",
            Description = "Test Description",
            Price = 100.50m,
            Code = "12345",
            ExpirationDate = DateTime.Today.AddDays(1),
            HasExpirationDate = true,
            ProductCategoryId = null
        };

    protected ProductUpdateRequest CreateValidProductUpdateRequest()
    {
        return new ProductUpdateRequest
        {
            ProductId = 1L,
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 150.75m,
            Code = "12345",
            ExpirationDate = DateTime.Today.AddDays(1),
            HasExpirationDate = true,
            ProductCategoryId = null
        };
    }
    
    protected ProductUpdateRequest CreateInvalidProductUpdateRequest()
    {
        return new ProductUpdateRequest
        {
            ProductId = 9999L,
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 150.75m,
            Code = "",
            ExpirationDate = DateTime.Today.AddDays(1),
            HasExpirationDate = false,
            ProductCategoryId = null
        };
    }

    protected ProductDeleteRequest CreateProductDeleteRequest()
    {
        return new ProductDeleteRequest()
        {
            ProductId = 1L
        };
    }
    
    protected ProductDeleteRequest CreateInvalideProductDeleteRequest()
    {
        return new ProductDeleteRequest()
        {
            ProductId = 0L
        };
    }
}