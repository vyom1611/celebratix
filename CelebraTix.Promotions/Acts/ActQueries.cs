using CelebraTix.Promotions.Data;
using Microsoft.EntityFrameworkCore;

namespace CelebraTix.Promotions.Acts;

public class ActQueries
{
    private readonly PromotionDataContext repository;

    public ActQueries(PromotionDataContext repository)
    {
        this.repository = repository;
    }

    public async Task<List<ActInfo>> ListActs()
    {
        var acts = await repository.Act
            .Where(act => !act.Removed.Any())
            .ToListAsync();

        return acts.Select(act => MapActModel(act.ActGuid, GetLatestDescription(act.Descriptions)))
            .ToList();
    }

    public async Task<ActInfo> GetAct(Guid actGuid)
    {
        var act = await repository.Act
            .Where(act => act.ActGuid == actGuid)
            .SingleOrDefaultAsync();

        return act == null ? null : MapActModel(act.ActGuid, GetLatestDescription(act.Descriptions));
    }

    private ActDescription GetLatestDescription(ICollection<ActDescription> descriptions)
    {
        return descriptions.OrderByDescending(d => d.ModifiedDate).FirstOrDefault();
    }

    private static ActInfo MapActModel(Guid actGuid, ActDescription actDescription)
    {
        return new ActInfo
        {
            ActGuid = actGuid,
            Title = actDescription?.Title,
            ImageHash = actDescription?.ImageHash,
            LastModifiedTicks = actDescription?.ModifiedDate.Ticks ?? 0
        };
    }
}