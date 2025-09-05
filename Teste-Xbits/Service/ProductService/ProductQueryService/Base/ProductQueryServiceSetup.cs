using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.Service.ProductService.ProductQueryService.Base;

public class ProductQueryServiceSetup
{
    protected readonly Mock<INotificationHandler> NotificationHandler;
    protected readonly Mock<IValidate<Product>> Validator;
    protected readonly Mock<ILoggerHandler> LoggerHandler;
    protected readonly Mock<IProductRepository> ProductRepository;
    protected readonly Mock<IProductMapper> ProductMapper;
    protected readonly ApplicationService.Services.ProductService.ProductQueryService ProductQueryService;

    protected readonly PageParams DefaultPageParams;
    protected readonly Dictionary<string, string> Errors;

    protected ProductQueryServiceSetup()
    {
        NotificationHandler = new Mock<INotificationHandler>();
        Validator = new Mock<IValidate<Product>>();
        LoggerHandler = new Mock<ILoggerHandler>();
        ProductRepository = new Mock<IProductRepository>();
        ProductMapper = new Mock<IProductMapper>();

        DefaultPageParams = new PageParams();
        Errors = [];

        ProductQueryService = new ApplicationService.Services.ProductService.ProductQueryService(
            NotificationHandler.Object,
            Validator.Object,
            LoggerHandler.Object,
            ProductRepository.Object,
            ProductMapper.Object
        );
    }

    protected Product CreateValidProduct()
    {
        return new Product
        {
            Id = 1L,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.50m,
            Code = "PROD001",
            ExpirationDate = DateTime.Today.AddDays(30),
            ProductCategoryId = 1L,
            CreatedAt = DateTime.Today,
            UpdatedAt = DateTime.Today
        };
    }

    protected Product CreateProductWithoutExpirationDate()
    {
        return new Product
        {
            Id = 2L,
            Name = "Test Product 2",
            Description = "Test Description 2",
            Price = 200.00m,
            Code = "PROD002",
            ExpirationDate = null,
            ProductCategoryId = 1L,
            CreatedAt = DateTime.Today,
            UpdatedAt = DateTime.Today
        };
    }

    protected ProductResponse CreateValidProductResponse()
    {
        return new ProductResponse
        {
            Id = 1L,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100.50m,
            Code = "PROD001",
            ExpirationDate = DateTime.Today.AddDays(30),
            ProductCategory = new ProductCategoryResponse()
            {
                Name = "Categoria",
                Description = "Categoria",
                ProductCategoryCode = "809732",
                Id = 1L,
                Products = new List<ProductResponse>()
            }
        };
    }

    protected PageList<Product> CreateProductPageList()
    {
        var products = new List<Product>
        {
            CreateValidProduct(),
            CreateProductWithoutExpirationDate()
        };

        return new PageList<Product>(products, products.Count, 1, 10);
    }

    protected PageList<ProductResponse> CreateProductResponsePageList()
    {
        var productResponses = new List<ProductResponse>
        {
            CreateValidProductResponse()
        };

        return new PageList<ProductResponse>(productResponses, productResponses.Count, 1, 10);
    }

    protected void SetupProductRepositoryFindAllWithPaginationAsync(PageList<Product> pageList)
    {
        ProductRepository
            .Setup(x => x.FindAllWithPaginationAsync(
                It.IsAny<PageParams>(),
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(pageList);
    }

    protected void SetupProductMapperDomainToPaginationResponse(PageList<ProductResponse> response)
    {
        ProductMapper
            .Setup(x => x.DomainToPaginationResponse(It.IsAny<PageList<Product>>()))
            .Returns(response);
    }
    
    protected void SetupProductRepositoryFindByPredicateAsync(Product product)
    {
        ProductRepository
            .Setup(r => r.FindByPredicateAsync(
                It.IsAny<Expression<Func<Product, bool>>>(),
                It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>?>(),
                It.IsAny<bool>()))
            .ReturnsAsync(product);
    }

    protected void SetupProductMapperDomainToSimpleResponse(ProductResponse response)
    {
        ProductMapper
            .Setup(m => m.DomainToSimpleResponse(It.IsAny<Product>()))
            .Returns(response);
    }

    protected (string? namePrefix, string? descriptionPrefix, decimal? pricePrefix,
        string? productCodePrefix, bool? hasValidadeDatePrefix, string? expirationDate,
        long? productCategoryIdPrefix) CreateNoFilters()
    {
        return (null, null, null, null, null, null, null);
    }

    protected (string? namePrefix, string? descriptionPrefix, decimal? pricePrefix,
        string? productCodePrefix, bool? hasValidadeDatePrefix, string? expirationDate,
        long? productCategoryIdPrefix) CreateExpirationDateFilter()
    {
        return (null, null, null, null, null, "2024-12-31", null);
    }
}