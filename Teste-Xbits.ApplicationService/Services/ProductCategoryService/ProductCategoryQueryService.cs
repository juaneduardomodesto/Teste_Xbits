using System.Linq.Expressions;
using LinqKit;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductCategoryResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.ProductCategoryService;

public class ProductCategoryQueryService : ServiceBase<ProductCategory>, IProductCategoryQueryService
{
    private readonly INotificationHandler _notificationHandler;
    private readonly IValidate<ProductCategory> _validate;
    private readonly ILoggerHandler _loggerHandler;
    private readonly IProductCategoryMapper _productCategoryMapper;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductCategoryQueryService(
        INotificationHandler notificationHandler,
        IValidate<ProductCategory> validate,
        ILoggerHandler loggerHandler,
        IProductCategoryMapper productCategoryMapper,
        IProductCategoryRepository productCategoryRepository) 
        : base(notificationHandler, validate, loggerHandler)
    {
        _notificationHandler = notificationHandler;
        _validate = validate;
        _loggerHandler = loggerHandler;
        _productCategoryMapper = productCategoryMapper;
        _productCategoryRepository = productCategoryRepository;
    }
    
    public async Task<ProductCategoryResponse?> FindByIdAsync(long id)
    {
        var productCategory = await _productCategoryRepository.FindByPredicateAsync(x => x.Id == id);

        return productCategory is null
            ? null
            : _productCategoryMapper.DomainToSimpleResponse(productCategory);
    }

    public async Task<PageList<ProductCategoryResponse>> FindAllWithPaginationAsync(string? namePrefix, string? descriptionPrefix, string? codePrefix, PageParams pageParams)
    {
        try
        {
            Expression<Func<ProductCategory, bool>> predicate = PredicateBuilder.New<ProductCategory>(x => true);

            if (!string.IsNullOrEmpty(namePrefix))
            {
                predicate = predicate.And(e =>
                    e.Name.StartsWith(namePrefix));
            }

            if (!string.IsNullOrEmpty(descriptionPrefix))
            {
                predicate = predicate.And(e =>
                    e.Description.StartsWith(descriptionPrefix));
            }

            if (!string.IsNullOrEmpty(codePrefix))
            {
                predicate = predicate.And(e =>
                    e.ProductCategoryCode.StartsWith(codePrefix));
            }

            var productCategoryList = await _productCategoryRepository.FindAllWithPaginationAsync(
                pageParams,
                predicate
            );

            return !productCategoryList.Items.Any()
                ? new PageList<ProductCategoryResponse>()
                : _productCategoryMapper.DomainToPaginationUserResponse(productCategoryList);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}