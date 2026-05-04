using Manager.Abstractions.Persistence;
using Manager.Models.Entity.Model;
using Manager.Models.Entity.Searchable;
using Manager.Persistence.Core;

namespace Manager.Persistence.Services;

public class ModQueryService : BaseEntityQueryService<ManagerDatabaseContext, Mod, SearchableMod>
{
    public ModQueryService(ManagerDatabaseContext context) : base(context)
    {
    }

    protected override IQueryable<Mod> AddComplexQueryArguments(
        IQueryable<Mod> query, IComplexSearchable<SearchableMod> complex)
    {
        return query;
    }

    protected override IEnumerable<Mod> ApplyComplexNonDatabaseQueryArguments(
        IEnumerable<Mod> entities, IComplexSearchable<SearchableMod> complex)
    {
        return entities;
    }

    protected override IQueryable<Mod> GetBaseQuery()
    {
        return context.Mods.AsQueryable();
    }

    protected override IQueryable<Mod> AddQueryArguments(SearchableMod searchable, IQueryable<Mod> query)
    {
        if (!string.IsNullOrWhiteSpace(searchable.Title))
        {
            query = query.Where(x => x.Title.ToLower() == searchable.Title.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(searchable.Author))
        {
            query = query.Where(x => x.Author.ToLower() == searchable.Author.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(searchable.Description))
        {
            query = query.Where(x => x.Description.ToLower() == searchable.Description.ToLower());
        }

        return query;
    }
}
