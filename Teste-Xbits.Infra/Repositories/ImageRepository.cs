using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Teste_Xbits.Domain.Entities;
using Teste_Xbits.Domain.Enums;
using Teste_Xbits.Infra.Interfaces.RepositoryContracts;
using Teste_Xbits.Infra.ORM.Context;
using Teste_Xbits.Infra.Repositories.Base;

namespace Teste_Xbits.Infra.Repositories;

public sealed class ImageRepository(ApplicationContext dbContext)
    : RepositoryBase<ImageFiles>(dbContext), IImageRepository
{
    public async Task<bool> SaveAsync(ImageFiles imageFiles)
    {
        await DbSetContext.AddAsync(imageFiles);
        return await SaveInDatabaseAsync();
    }

    public Task<bool> DeleteAsync(ImageFiles imageFiles)
    {
        DetachedObject(imageFiles);
        DbSetContext.Remove(imageFiles);
        return SaveInDatabaseAsync();
    }

    public Task<bool> UpdateAsync(ImageFiles imageFiles)
    {
        DbSetContext.Update(imageFiles);
        return SaveInDatabaseAsync();
    }

    public Task<ImageFiles?> FindByPredicateAsync(
        Expression<Func<ImageFiles, bool>> predicate,
        bool asNoTracking = false)
    {
        IQueryable<ImageFiles> query = DbSetContext;

        if (asNoTracking)
            query = query.AsNoTracking();

        return query.OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<ImageFiles>> FindByEntityAsync(
        EEntityType entityType,
        long entityId)
    {
        return await DbSetContext
            .Where(x => x.EntityType == entityType && x.EntityId == entityId)
            .OrderByDescending(x => x.IsMain)
            .ThenBy(x => x.DisplayOrder)
            .ThenByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public Task<ImageFiles?> GetMainImageAsync(
        EEntityType entityType,
        long entityId)
    {
        return DbSetContext
            .Where(x => x.EntityType == entityType 
                     && x.EntityId == entityId 
                     && x.IsMain)
            .FirstOrDefaultAsync();
    }

// Infra/Repositories/ImageRepository.cs

    public async Task<bool> SetMainImageAsync(
        long imageId,
        EEntityType entityType,
        long entityId)
    {
        // 1. Remover IsMain de todas as imagens (COM AsNoTracking)
        await UnsetMainImagesAsync(entityType, entityId, imageId);

        // 2. Buscar a imagem alvo SEM tracking
        var image = await DbSetContext
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == imageId);
    
        if (image == null) return false;

        // 3. Criar nova instância com IsMain = true
        var updatedImage = image with 
        { 
            IsMain = true, 
            UpdatedAt = DateTime.Now 
        };
    
        // 4. Atualizar
        DbSetContext.Update(updatedImage);
    
        return await SaveInDatabaseAsync();
    }

    public async Task<bool> UnsetMainImagesAsync(
        EEntityType entityType,
        long entityId,
        long exceptImageId)
    {
        // IMPORTANTE: Buscar SEM tracking
        var images = await DbSetContext
            .AsNoTracking() // ⬅️ ADICIONAR ISSO
            .Where(x => x.EntityType == entityType 
                        && x.EntityId == entityId 
                        && x.IsMain 
                        && x.Id != exceptImageId)
            .ToListAsync();

        foreach (var image in images)
        {
            var updatedImage = image with 
            { 
                IsMain = false, 
                UpdatedAt = DateTime.Now 
            };
            DbSetContext.Update(updatedImage);
        }

        return images.Any() && await SaveInDatabaseAsync();
    }
}