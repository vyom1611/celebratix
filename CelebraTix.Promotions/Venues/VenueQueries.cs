using CelebraTix.Promotions.Data;
using Microsoft.EntityFrameworkCore;

namespace CelebraTix.Promotions.Venues;

public class VenueQueries
{
    private readonly PromotionDataContext context;

    public VenueQueries(PromotionDataContext context)
    {
        this.context = context;
    }

    public async Task<List<VenueInfo>> ListVenues()
    {
        var venues = await context.Venues
            .Select(venue => new
            {
                venue.VenueGuid,
                Description = venue.Descriptions.OrderByDescending(d => d.ModifiedDate).FirstOrDefault()
            })
            .ToListAsync()
            .ConfigureAwait(false);

        return venues.Select(row => MapVenue(row.VenueGuid, row.Description)).ToList();
    }

    public async Task<VenueInfo> GetVenue(Guid venueGuid)
    {
        var venue = await context.Venues
            .Where(venue => venue.VenueGuid == venueGuid)
            .Select(venue => new { venue.VenueGuid })
            .SingleOrDefaultAsync()
            .ConfigureAwait(false);

        return venue == null ? null : MapVenue(venue.VenueGuid, null);
    }

    private VenueInfo MapVenue(Guid venueGuid, VenueDescription venueDescription)
        => new VenueInfo
        {
            VenueGuid = venueGuid,
            Name = venueDescription?.Name,
            City = venueDescription?.City,
            LastModifiedTicks = venueDescription?.ModifiedDate.Ticks ?? 0
        };
}