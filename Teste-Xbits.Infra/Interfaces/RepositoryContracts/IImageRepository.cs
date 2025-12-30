using System.Linq.Expressions;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;

namespace Teste_Xbits.Infra.Interfaces.RepositoryContracts;

public interface IImageRepository
{
    Task<bool> SaveAsync(ImageFiles imageFiles);
    Task<bool> DeleteAsync(ImageFiles imageFiles);
    Task<bool> UpdateAsync(ImageFiles imageFiles);
    
    Task<ImageFiles?> FindByPredicateAsync(
        Expression<Func<ImageFiles, bool>> predicate,
        bool asNoTracking = false);
    
    Task<IEnumerable<ImageFiles>> FindByEntityAsync(
        EEntityType entityType,
        long entityId);
    
    Task<ImageFiles?> GetMainImageAsync(
        EEntityType entityType,
        long entityId);
    
    Task<bool> SetMainImageAsync(
        long imageId,
        EEntityType entityType,
        long entityId);
    
    Task<bool> UnsetMainImagesAsync(
        EEntityType entityType,
        long entityId,
        long exceptImageId);
}