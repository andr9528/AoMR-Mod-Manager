using Manager.Abstractions.Persistence;
using Manager.Models.Entity.Model;
using Manager.Models.Entity.Searchable;
using Manager.Persistence.Core;

namespace Manager.Persistence.Services;

public class PlaysetQueryService : BaseEntityQueryService<ManagerDatabaseContext, Playset, SearchablePlayset>
{
    public PlaysetQueryService(ManagerDatabaseContext context) : base(context)
    {
    }

    protected override IQueryable<Playset> AddComplexQueryArguments(
        IQueryable<Playset> query, IComplexSearchable<SearchablePlayset> complex)
    {
        return query;
    }

    protected override IEnumerable<Playset> ApplyComplexNonDatabaseQueryArguments(
        IEnumerable<Playset> entities, IComplexSearchable<SearchablePlayset> complex)
    {
        return entities;
    }

    protected override IQueryable<Playset> GetBaseQuery()
    {
        return context.Playsets.AsQueryable();
    }

    protected override IQueryable<Playset> AddQueryArguments(SearchablePlayset searchable, IQueryable<Playset> query)
    {
        if (!string.IsNullOrWhiteSpace(searchable.Name))
        {
            query = query.Where(x => x.Name.ToLower() == searchable.Name.ToLower());
        }

        return query;
    }
}
