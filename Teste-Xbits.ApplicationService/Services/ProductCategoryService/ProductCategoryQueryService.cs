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

public class ProductCategoryQueryService(
    INotificationHandler notificationHandler,
    IValidate<ProductCategory> validate,
    ILoggerHandler loggerHandler,
    IProductCategoryMapper productCategoryMapper,
    IProductCategoryRepository productCategoryRepository)
    : ServiceBase<ProductCategory>(notificationHandler, validate, loggerHandler), IProductCategoryQueryService
{
    private readonly ILoggerHandler _loggerHandler = loggerHandler;

    public async Task<ProductCategoryResponse?> FindByIdAsync(long id)
    {
        var productCategory = await productCategoryRepository.FindByPredicateAsync(x => x.Id == id);

        return productCategory is null
            ? null
            : productCategoryMapper.DomainToSimpleResponse(productCategory);
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

            var productCategoryList = await productCategoryRepository.FindAllWithPaginationAsync(
                pageParams,
                predicate
            );

            return !productCategoryList.Items.Any()
                ? new PageList<ProductCategoryResponse>()
                : productCategoryMapper.DomainToPaginationResponse(productCategoryList);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}