using CelebraTix.Promotions.Data;
using Microsoft.EntityFrameworkCore;

namespace CelebraTix.Promotions.Acts;

public class ActCommands
{
    private readonly PromotionDataContext repository;

    public ActCommands(PromotionDataContext repository)
    {
        this.repository = repository;
    }
    
    public async Task SaveAct(ActInfo actModel)
        {
            var act = await GetOrInsertActAsync(actModel.ActGuid);
            var lastActDescription = GetLatestActDescription(act);

            if (ShouldAddNewDescription(lastActDescription, actModel))
            {
                ValidateConcurrency(lastActDescription, actModel);
                await AddActDescriptionAsync(act, actModel);
            }
        }

        public async Task RemoveAct(Guid actGuid)
        {
            var act = await GetOrInsertActAsync(actGuid);
            await AddActRemovedAsync(act);
        }

        private async Task<Act> GetOrInsertActAsync(Guid actGuid)
        {
            return await repository.GetOrInsertAct(actGuid);
        }

        private ActDescription GetLatestActDescription(Act act)
        {
            return act.Descriptions.OrderByDescending(d => d.ModifiedDate).FirstOrDefault();
        }

        private bool ShouldAddNewDescription(ActDescription lastDescription, ActInfo actModel)
        {
            return lastDescription == null ||
                   lastDescription.Title != actModel.Title ||
                   lastDescription.ImageHash != actModel.ImageHash;
        }

        private void ValidateConcurrency(ActDescription lastDescription, ActInfo actModel)
        {
            var modifiedTicks = lastDescription?.ModifiedDate.Ticks ?? 0;
            if (modifiedTicks != actModel.LastModifiedTicks)
            {
                throw new DbUpdateConcurrencyException("A new update has occurred since you loaded the page. Please refresh and try again.");
            }
        }

        private async Task AddActDescriptionAsync(Act act, ActInfo actModel)
        {
            await repository.AddAsync(new ActDescription
            {
                ModifiedDate = DateTime.UtcNow,
                Act = act,
                Title = actModel.Title,
                ImageHash = actModel.ImageHash
            });
            await repository.SaveChangesAsync();
        }

        private async Task AddActRemovedAsync(Act act)
        {
            await repository.AddAsync(new ActRemoved
            {
                Act = act,
                RemovedDate = DateTime.UtcNow
            });
            await repository.SaveChangesAsync();
        }
}