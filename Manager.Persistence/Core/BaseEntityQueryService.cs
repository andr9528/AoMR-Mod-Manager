using Manager.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Manager.Persistence.Core;

public abstract class
    BaseEntityQueryService<TContext, TEntity, TSearchable> : IEntityQueryService<TEntity, TSearchable>
    where TContext : BaseDatabaseContext<TContext>
    where TEntity : class, IEntity
    where TSearchable : class, ISearchable, new()
{
    protected readonly TContext context;

    protected BaseEntityQueryService(TContext context)
    {
        this.context = context;
    }

    /// <inheritdoc />
    public async Task AddEntity(TEntity entity, bool saveImmediately = true)
    {
        await context.AddAsync(entity);

        if (saveImmediately)
            await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task AddEntities(IEnumerable<TEntity> entities, bool saveImmediately = true)
    {
        await context.AddRangeAsync(entities);

        if (saveImmediately)
            await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetEntity(TSearchable searchable)
    {
        return (await BuildQuery(searchable).ToListAsync()).FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetEntityComplex(IComplexSearchable<TSearchable> complex)
    {
        var basicQuery = BuildQuery(complex.Searchable);
        var databaseResult = await AddComplexQueryArguments(basicQuery, complex).ToListAsync();

        return ApplyComplexNonDatabaseQueryArguments(databaseResult, complex).FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetEntities(TSearchable searchable)
    {
        return await BuildQuery(searchable).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetEntitiesComplex(IComplexSearchable<TSearchable> complex)
    {
        var basicQuery = BuildQuery(complex.Searchable);
        var databaseResult = await AddComplexQueryArguments(basicQuery, complex).ToListAsync();

        return ApplyComplexNonDatabaseQueryArguments(databaseResult, complex);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllEntities()
    {
        return await GetBaseQuery().ToListAsync();
    }

    /// <inheritdoc />
    public async Task UpdateEntity(TEntity entity, bool saveImmediately = true)
    {
        context.Update(entity);

        if (saveImmediately)
            await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateEntities(IEnumerable<TEntity> entities, bool saveImmediately = true)
    {
        context.UpdateRange(entities);

        if (saveImmediately)
            await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteEntity(TSearchable searchable, bool saveImmediately = true)
    {
        TEntity entity = await GetEntity(searchable);
        context.Remove(entity);

        if (saveImmediately)
            await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeleteEntityById(int id, bool saveImmediately = true)
    {
        await DeleteEntity(new TSearchable { Id = id, }, saveImmediately);
    }

    private IQueryable<TEntity> BuildQuery(TSearchable searchable)
    {
        var query = GetBaseQuery();

        if (searchable.Id != 0)
            query = query.Where(x => x.Id == searchable.Id);

        query = AddQueryArguments(searchable, query);

        return query;
    }

    /// <summary>
    /// Add Query Arguments from the supplied <see cref="IComplexSearchable{TSearchable}"/>.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="complex"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">Thrown when there are no implementation of <see cref="IComplexSearchable{TSearchable}"/></exception>
    /// <exception cref="InvalidOperationException">Thrown when supplied properties would guarantee no results.</exception>
    protected abstract IQueryable<TEntity> AddComplexQueryArguments(
        IQueryable<TEntity> query, IComplexSearchable<TSearchable> complex);

    protected abstract IEnumerable<TEntity> ApplyComplexNonDatabaseQueryArguments(
        IEnumerable<TEntity> entities, IComplexSearchable<TSearchable> complex);

    /// <summary>
    /// Gets the base <see cref="IQueryable{TEntity}"/> from the <see cref="TContext"/>.
    /// </summary>
    /// <returns></returns>
    protected abstract IQueryable<TEntity> GetBaseQuery();


    /// <summary>
    /// Add Query Arguments from the supplied <see cref="TSearchable"/>.
    /// </summary>
    /// <param name="searchable"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    protected abstract IQueryable<TEntity> AddQueryArguments(TSearchable searchable, IQueryable<TEntity> query);
}
