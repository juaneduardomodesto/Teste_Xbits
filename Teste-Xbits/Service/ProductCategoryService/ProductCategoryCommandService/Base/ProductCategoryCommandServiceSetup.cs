using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Request.ProductCategoryRequest;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.Service.ProductCategoryService.ProductCategoryCommandService.Base
{
    public class ProductCategoryCommandServiceSetup
    {
        protected readonly Mock<INotificationHandler> NotificationHandler;
        protected readonly Mock<IValidate<ProductCategory>> Validator;
        protected readonly Mock<ILoggerHandler> LoggerHandler;
        protected readonly Mock<IProductCategoryMapper> ProductCategoryMapper;
        protected readonly Mock<IProductCategoryRepository> ProductCategoryRepository;
        protected readonly Dictionary<string, string> Errors;
        protected readonly ValidationResponse ValidationResponse;
        protected readonly ValidationResponse ValidationResponseWithErrors;
        protected readonly ApplicationService.Services.ProductCategoryService.ProductCategoryCommandService ProductCategoryCommandService;

        public ProductCategoryCommandServiceSetup()
        {
            NotificationHandler = new Mock<INotificationHandler>();
            Validator = new Mock<IValidate<ProductCategory>>();
            LoggerHandler = new Mock<ILoggerHandler>();
            ProductCategoryMapper = new Mock<IProductCategoryMapper>();
            ProductCategoryRepository = new Mock<IProductCategoryRepository>();
            
            Errors = [];
            ValidationResponse = ValidationResponse.CreateResponse(Errors);
            ValidationResponseWithErrors = ValidationResponse.CreateResponse(new Dictionary<string, string> 
            { 
                { "Error", "Validation failed" } 
            });
            
            Validator.Setup(x => x.ValidationAsync(It.IsAny<ProductCategory>()))
                     .ReturnsAsync(ValidationResponse);

            ProductCategoryCommandService = new ApplicationService.Services.ProductCategoryService.ProductCategoryCommandService(
                NotificationHandler.Object,
                Validator.Object,
                LoggerHandler.Object,
                ProductCategoryMapper.Object,
                ProductCategoryRepository.Object);
        }

        protected void SetupValidationSuccess() 
        {
            Validator.Setup(x => x.ValidationAsync(It.IsAny<ProductCategory>()))
                     .ReturnsAsync(ValidationResponse);
        }

        protected void SetupValidationFailure() 
        {
            Validator.Setup(x => x.ValidationAsync(It.IsAny<ProductCategory>()))
                     .ReturnsAsync(ValidationResponseWithErrors);
        }

        protected void CreateNotification() => Errors.Add("Error", "Error");
        
        protected UserCredential CreateUserCredential() => new UserCredential 
        { 
            Id = Guid.NewGuid(), 
            Roles = [] 
        };

        protected ProductCategoryRegisterRequest CreateValidProductCategoryRegisterRequest() => new ProductCategoryRegisterRequest
        {
            Name = "Test Category",
            Description = "Test Description",
            Code = "TEST001"
        };

        protected ProductCategoryRegisterRequest CreateInvalidProductCategoryRegisterRequest() => new ProductCategoryRegisterRequest
        {
            Name = "",
            Description = "",
            Code = ""
        };

        protected ProductCategoryUpdateRequest CreateValidProductCategoryUpdateRequest() => new ProductCategoryUpdateRequest
        {
            Id = 1,
            Name = "Updated Category",
            Description = "Updated Description",
            Code = "UPD001"
        };

        protected ProductCategoryUpdateRequest CreateInvalidProductCategoryUpdateRequest() => new ProductCategoryUpdateRequest
        {
            Id = 0,
            Name = "",
            Description = "",
            Code = ""
        };

        protected ProductCategoryDeleteRequest CreateValidProductCategoryDeleteRequest() => new ProductCategoryDeleteRequest
        {
            Id = 1
        };

        protected ProductCategoryDeleteRequest CreateInvalidProductCategoryDeleteRequest() => new ProductCategoryDeleteRequest
        {
            Id = 0
        };

        protected ProductCategory CreateValidProductCategory() => new ProductCategory
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description",
            ProductCategoryCode = "TEST001"
        };

        protected ProductCategory CreateInvalidProductCategory() => new ProductCategory
        {
            Id = 0,
            Name = "",
            Description = "",
            ProductCategoryCode = ""
        };
    }
}