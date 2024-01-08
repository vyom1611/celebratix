using CelebraTix.Promotions.Data;
using Microsoft.EntityFrameworkCore;

namespace CelebraTix.Promotions.Venues;

public class VenueQueries
{
    private readonly PromotionDataContext repository;

    public VenueQueries(PromotionDataContext repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<List<VenueInfo>> ListVenues()
    {
        var venues = await repository.Venues
            .Where(venue => !venue.Removed.Any())
            .Select(venue => new
            {
                venue.VenueGuid,
                Descriptions = venue.Descriptions.OrderByDescending(d => d.ModifiedDate).FirstOrDefault()
            })
            .ToListAsync();

        return venues.Select(v => new VenueInfo
        {
            VenueGuid = v.VenueGuid,
            Name = v.Descriptions?.Name,
            City = v.Descriptions?.City,
            LastModifiedTicks = v.Descriptions?.ModifiedDate.Ticks ?? 0
        }).ToList();
    }

    public async Task<VenueInfo> GetVenue(Guid venueGuid)
    {
        var result = await repository.Venues
            .Where(venue => venue.VenueGuid == venueGuid && !venue.Removed.Any())
            .Select(venue => new
            {
                venue.VenueGuid,
                Description = venue.Descriptions.OrderByDescending(d => d.ModifiedDate).FirstOrDefault()
            })
            .SingleOrDefaultAsync();

        if (result == null) return null;

        return new VenueInfo
        {
            VenueGuid = result.VenueGuid,
            Name = result.Description?.Name,
            City = result.Description?.City,
            LastModifiedTicks = result.Description?.ModifiedDate.Ticks ?? 0
        };
    }
}