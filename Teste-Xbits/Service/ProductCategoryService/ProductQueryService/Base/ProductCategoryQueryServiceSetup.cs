using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Services.ProductCategoryService;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Domain.Handlers.ValidationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.Service.ProductCategoryService.ProductQueryService.Base;

public class ProductCategoryQueryServiceSetup
{
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<ProductCategory>> Validator;
    protected readonly Mock<ILoggerHandler> LoggerHandler;
    protected readonly Mock<IProductCategoryMapper> ProductCategoryMapper;
    protected readonly Mock<IProductCategoryRepository> ProductCategoryRepository;
    protected readonly Dictionary<string, string> Errors;
    protected readonly ValidationResponse ValidationResponse;
    protected readonly ProductCategoryQueryService ProductCategoryQueryService;

    public ProductCategoryQueryServiceSetup()
    {
        NotificationHandler = new Mock<INotificationHandler>();
        Validator = new Mock<IValidate<ProductCategory>>();
        LoggerHandler = new Mock<ILoggerHandler>();
        ProductCategoryMapper = new Mock<IProductCategoryMapper>();
        ProductCategoryRepository = new Mock<IProductCategoryRepository>();
        
        Errors = [];
        ValidationResponse = ValidationResponse.CreateResponse(Errors);

        ProductCategoryQueryService = new ProductCategoryQueryService(
            NotificationHandler.Object,
            Validator.Object,
            LoggerHandler.Object,
            ProductCategoryMapper.Object,
            ProductCategoryRepository.Object);
    }
    
    protected ProductCategory CreateValidProductCategory() => new ProductCategory
    {
        Id = 1,
        Name = "Test Category",
        Description = "Test Description",
        ProductCategoryCode = "TEST001"
    };

    protected ProductCategoryResponse CreateValidProductCategoryResponse() => new ProductCategoryResponse
    {
        Name = "Test Category",
        Description = "Test Description",
        ProductCategoryCode = "TEST001"
    };
    
    protected PageList<ProductCategory> CreateProductPageList()
    {
        var productCategories = new List<ProductCategory>
        {
            CreateValidProductCategory(),
            CreateValidProductCategory()
        };

        return new PageList<ProductCategory>(productCategories, productCategories.Count, 1, 10);
    }
    
    protected PageList<ProductCategoryResponse> CreateProductCategoryResponsePageList()
    {
        var productCategoryResponses = new List<ProductCategoryResponse>
        {
            CreateValidProductCategoryResponse(),
        };

        return new PageList<ProductCategoryResponse>(productCategoryResponses, productCategoryResponses.Count, 1, 10);
    }
    
    protected void SetupProductCategoryRepositoryFindAllWithPaginationAsync(PageList<ProductCategory> pageList)
    {
        ProductCategoryRepository
            .Setup(x => x.FindAllWithPaginationAsync(
                It.IsAny<PageParams>(),
                It.IsAny<Expression<Func<ProductCategory, bool>>>(),
                It.IsAny<Func<IQueryable<ProductCategory>, IQueryable<ProductCategory>>?>()))
            .ReturnsAsync(pageList);
    }

    protected void SetupProductCategoryMapperDomainToPaginationResponse(PageList<ProductCategoryResponse> response)
    {
        ProductCategoryMapper
            .Setup(x => x.DomainToPaginationResponse(It.IsAny<PageList<ProductCategory>>()))
            .Returns(response);
    }
    
    protected void SetupProductCategoryRepositoryFindByPredicateAsync(ProductCategory productCategory)
    {
        ProductCategoryRepository
            .Setup(r => r.FindByPredicateAsync(
                It.IsAny<Expression<Func<ProductCategory, bool>>>(),
                It.IsAny<Func<IQueryable<ProductCategory>, IQueryable<ProductCategory>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(productCategory);
    }

    protected void SetupProductCategoryMapperDomainToSimpleResponse(ProductCategoryResponse productCategoryResponse)
    {
        ProductCategoryMapper
            .Setup(m => m.DomainToSimpleResponse(It.IsAny<ProductCategory>()))
            .Returns(productCategoryResponse);
    }
}