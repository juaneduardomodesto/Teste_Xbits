using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Teste_Xbits.ApplicationService.DataTransferObjects.Response.ProductResponse;
using Teste_Xbits.ApplicationService.Interfaces.MapperContracts;
using Teste_Xbits.ApplicationService.Interfaces.ServiceContracts;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Handlers.PaginationHandler;
using Teste_Xbits.Domain.Interface;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;

namespace Teste_Xbits.ApplicationService.Services.ProductService;

public class ProductQueryService(
    INotificationHandler notificationHandler,
    IValidate<Product> validate,
    ILoggerHandler logger,
    IProductRepository productRepository,
    IProductMapper productMapper)
    : ServiceBase<Product>(notificationHandler, validate, logger), IProductQueryService
{
    public async Task<ProductResponse?> FindByIdAsync(long id)
    {
        var product = await productRepository.FindByPredicateAsync(x => x.Id == id,
            include: x => x.Include(y => y.ProductCategory));

        return product is null
            ? null
            : productMapper.DomainToSimpleResponse(product);
    }

    public async Task<PageList<ProductResponse>> FindAllWithPaginationAsync(
        string? namePrefix,
        string? descriptionPrefix,
        decimal? pricePrefix,
        string? productCodePrefix,
        bool? hasValidadeDatePrefix,
        string? expirationDate,
        long? productCategoryIdPrefix,
        PageParams pageParams)
    {
        try
        {
            Expression<Func<Product, bool>> predicate = PredicateBuilder.New<Product>(x => true);

            if (!string.IsNullOrEmpty(namePrefix))
            {
                predicate = predicate.And(e => e.Name.StartsWith(namePrefix));
            }

            if (!string.IsNullOrEmpty(descriptionPrefix))
            {
                predicate = predicate.And(e =>
                    e.Description != null &&
                    e.Description.StartsWith(descriptionPrefix));
            }

            if (pricePrefix.HasValue)
            {
                predicate = predicate.And(e => e.Price == pricePrefix.Value);
            }

            if (!string.IsNullOrEmpty(productCodePrefix))
            {
                predicate = predicate.And(e => e.Code.StartsWith(productCodePrefix));
            }

            if (hasValidadeDatePrefix.HasValue)
            {
                if (hasValidadeDatePrefix.Value)
                {
                    predicate = predicate.And(e => e.ExpirationDate != null);
                }
                else
                {
                    predicate = predicate.And(e => e.ExpirationDate == null);
                }
            }
            
            if (!string.IsNullOrEmpty(expirationDate))
            {
                if (Regex.IsMatch(expirationDate, @"^\d{4}-\d{2}-\d{2}$"))
                {
                    if (DateTime.TryParse(expirationDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var filterDate))
                    {
                        predicate = predicate.And(e =>
                            e.ExpirationDate != null &&
                            e.ExpirationDate.Value.Date == filterDate.Date);
                    }
                    else
                    {
                        predicate = predicate.And(e =>
                            e.ExpirationDate != null &&
                            e.ExpirationDate.Value.ToString("yyyy-MM-dd").StartsWith(expirationDate));
                    }
                }
            }

            if (productCategoryIdPrefix.HasValue)
            {
                predicate = predicate.And(e => e.ProductCategoryId == productCategoryIdPrefix.Value);
            }

            var productList = await productRepository.FindAllWithPaginationAsync(
                pageParams,
                predicate,
                x => x.Include(x => x.ProductCategory)
            );

            return !productList.Items.Any()
                ? new PageList<ProductResponse>()
                : productMapper.DomainToPaginationResponse(productList);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}