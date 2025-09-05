using System.Linq.Expressions;
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

            Validator.Setup(x => x.ValidationAsync(It.IsAny<ProductCategory>()))
                     .ReturnsAsync(ValidationResponse);

            ProductCategoryCommandService = new ApplicationService.Services.ProductCategoryService.ProductCategoryCommandService(
                NotificationHandler.Object,
                Validator.Object,
                LoggerHandler.Object,
                ProductCategoryMapper.Object,
                ProductCategoryRepository.Object);
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

        protected ProductCategoryDeleteRequest CreateValidProductCategoryDeleteRequest() => new ProductCategoryDeleteRequest
        {
            Id = 1
        };
        
        protected ProductCategoryUpdateRequest CreateValidProductCategoryUpdateRequest() => new ProductCategoryUpdateRequest
        {
            Id = 1L,
            Name = "Test Category",
            Description = "Test Description",
            Code = "TEST001"
        };

        protected ProductCategory CreateValidProductCategory() => new ProductCategory
        {
            Id = 1,
            Name = "Test Category",
            Description = "Test Description",
            ProductCategoryCode = "TEST001"
        };
        
        protected void SetupProductCategoryMapperDtoRegisterToDomain(ProductCategoryRegisterRequest productCategoryRequest, 
            ProductCategory productCategory)
        {
            ProductCategoryMapper.Setup(
                    x => x.DtoRegisterToDomain(productCategoryRequest))
                .Returns(productCategory);
        }

        protected void SetupValidatorValidationAsync()
        {
            Validator.Setup(x => x.ValidationAsync(It.IsAny<ProductCategory>()))
                .ReturnsAsync(ValidationResponse);
        }
        
        protected void SetupProductCategoryRepositoryUpdateAsync(ProductCategory productCategory, bool returnValue)
        {
            ProductCategoryRepository.Setup(x => x.UpdateAsync(productCategory))
                .ReturnsAsync(returnValue);
        }

        protected void SetupProductCategoryRepositorySaveAsync(ProductCategory productCategory, bool returnValue)
        {
            ProductCategoryRepository.Setup(x => x.SaveAsync(productCategory))
                .ReturnsAsync(returnValue);
        }
        
        protected void SetupProductCategoryRepositoryDeleteAsync(ProductCategory productCategory, bool returnValue)
        {
            ProductCategoryRepository.Setup(x => x.DeleteAsync(productCategory))
                .ReturnsAsync(returnValue);
        }

        protected void SetupLoggerHandlerCreateLogger()
        {
            LoggerHandler.Setup(x => x.CreateLogger(It.IsAny<DomainLogger>()));
        }

        protected void SetupProductCategoryFindPredicateAsync(ProductCategory existingProductCategory)
        {
            ProductCategoryRepository.Setup(x => x.FindByPredicateAsync(
                    It.IsAny<Expression<Func<ProductCategory, bool>>>(), null, false))
                .ReturnsAsync(existingProductCategory);
        }

        protected void SetupProductCategoryRepositoryFindByPredicateAsyncFindNull(ProductCategoryUpdateRequest dtoUpdate)
        {
            ProductCategoryRepository
                .Setup(x => x.FindByPredicateAsync(
                    x => x.Id == dtoUpdate.Id, null, true))
                .ReturnsAsync((ProductCategory)null!);
        }

        protected void SetupProductCategoryRepositoryFindByPredicateAsync(
            ProductCategoryUpdateRequest dtoUpdate, ProductCategory existingProductCategory)
        {
            ProductCategoryRepository
                .Setup(x => x.FindByPredicateAsync(
                    x => x.Id == dtoUpdate.Id, null, true))
                .ReturnsAsync(existingProductCategory);
        }
        
        protected void SetupProductCategoryMapperDtoUpdateToDomain(
            ProductCategoryUpdateRequest dtoUpdate, long id, ProductCategory updatedProductCategory)
        {
            ProductCategoryMapper
                .Setup(x => x.DtoUpdateToDomain(dtoUpdate, id))
                .Returns(updatedProductCategory);
        }

        protected void SetupProductCategoryMapperDtoUpdateToDomain(
            ProductCategoryUpdateRequest dtoUpdate, 
            ProductCategory existingProductCategory, 
            ProductCategory updatedProductCategory)
        {
            ProductCategoryMapper
                .Setup(x => x.DtoUpdateToDomain(dtoUpdate, existingProductCategory.Id))
                .Returns(updatedProductCategory);
        }
    }
}