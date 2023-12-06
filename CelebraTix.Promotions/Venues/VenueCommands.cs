using CelebraTix.Promotions.Data;

namespace CelebraTix.Promotions.Venues;

public class VenueCommands
{
    private readonly PromotionDataContext repository;

    public VenueCommands(PromotionDataContext repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task SaveVenue(VenueInfo venueModel)
    {
        var venue = await GetOrInsertVenueAsync(venueModel.VenueGuid);
        AddVenueDescription(venue, venueModel);
        await SaveChangesAsync();
    }

    public async Task DeleteVenue(Guid venueGuid)
    {
        var venue = await GetOrInsertVenueAsync(venueGuid);
        AddVenueRemoved(venue);
        await SaveChangesAsync();
    }

    private async Task<Venue> GetOrInsertVenueAsync(Guid venueGuid)
    {
        return await repository.GetOrInsertVenue(venueGuid)
            .ConfigureAwait(false) ?? throw new InvalidOperationException("Venue could not be found or created.");
    }

    private void AddVenueDescription(Venue venue, VenueInfo venueModel)
    {
        repository.Add(new VenueDescription
        {
            ModifiedDate = DateTime.UtcNow,
            Venue = venue,
            Name = venueModel.Name,
            City = venueModel.City
        });
    }

    private void AddVenueRemoved(Venue venue)
    {
        repository.Add(new VenueRemoved
        {
            Venue = venue,
            RemovedDate = DateTime.UtcNow
        });
    }

    private async Task SaveChangesAsync()
    {
        await repository.SaveChangesAsync().ConfigureAwait(false);
    }
}