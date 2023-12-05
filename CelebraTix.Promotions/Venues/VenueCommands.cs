using CelebraTix.Promotions.Data;

namespace CelebraTix.Promotions.Venues
{
    public class VenueCommands
    {
        private readonly PromotionDataContext repository;

        public VenueCommands(PromotionDataContext repository)
        {
            this.repository = repository;
        }

        public async Task SaveVenue(VenueInfo venueModel)
        {
            var venue = await GetOrInsertVenueAsync(venueModel.VenueGuid);
            await AddVenueDescriptionAsync(venue, venueModel);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteVenue(Guid venueGuid)
        {
            var venue = await GetOrInsertVenueAsync(venueGuid);
            await AddVenueRemovedAsync(venue);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task<Venue> GetOrInsertVenueAsync(Guid venueGuid)
        {
            return await repository.GetOrInsertVenue(venueGuid).ConfigureAwait(false);
        }

        private async Task AddVenueDescriptionAsync(Venue venue, VenueInfo venueModel)
        {
            await repository.AddAsync(new VenueDescription
            {
                ModifiedDate = DateTime.UtcNow,
                Venue = venue,
                Name = venueModel.Name,
                City = venueModel.City
            }).ConfigureAwait(false);
        }

        private async Task AddVenueRemovedAsync(Venue venue)
        {
            await repository.AddAsync(new VenueRemoved
            {
                Venue = venue,
                RemovedDate = DateTime.UtcNow
            }).ConfigureAwait(false);
        }
    }
}