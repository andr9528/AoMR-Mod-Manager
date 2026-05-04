using Manager.Abstractions.Persistence;
using Manager.Models.Entity.Model;
using Manager.Models.Entity.Searchable;
using Manager.Persistence.Core;

namespace Manager.Persistence.Services;

public class PlaysetModQueryService : BaseEntityQueryService<ManagerDatabaseContext, PlaysetMod, SearchablePlaysetMod>
{
    public PlaysetModQueryService(ManagerDatabaseContext context) : base(context)
    {
    }

    protected override IQueryable<PlaysetMod> AddComplexQueryArguments(
        IQueryable<PlaysetMod> query, IComplexSearchable<SearchablePlaysetMod> complex)
    {
        return query;
    }

    protected override IEnumerable<PlaysetMod> ApplyComplexNonDatabaseQueryArguments(
        IEnumerable<PlaysetMod> entities, IComplexSearchable<SearchablePlaysetMod> complex)
    {
        return entities;
    }

    protected override IQueryable<PlaysetMod> GetBaseQuery()
    {
        return context.PlaysetMods.AsQueryable();
    }

    protected override IQueryable<PlaysetMod> AddQueryArguments(
        SearchablePlaysetMod searchable, IQueryable<PlaysetMod> query)
    {
        if (searchable.PlaysetId != 0)
        {
            query = query.Where(x => x.PlaysetId == searchable.PlaysetId);
        }

        if (searchable.ModId != 0)
        {
            query = query.Where(x => x.ModId == searchable.ModId);
        }

        return query;
    }
}
