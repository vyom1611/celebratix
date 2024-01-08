using CelebraTix.Promotions.Data;
using CelebraTix.Promotions.Venues;
using Microsoft.EntityFrameworkCore;

namespace CelebraTix.Promotions.Shows;

public class ShowQueries
{
    private PromotionDataContext repository;

    public ShowQueries(PromotionDataContext promotionDataContext)
    {
        this.repository = promotionDataContext;
    }

    public async Task<List<ShowInfo>> ListShows(Guid actGuid)
    {
        var result = await repository.Show.Where(show =>
                show.Act.ActGuid == actGuid && !show.Cancelled.Any())
            .Select(show => new
            {
                VenueGuid = show.Venue.VenueGuid,
                VenueDescription = show.Venue.Descriptions
                    .OrderByDescending(d => d.ModifiedDate)
                    .FirstOrDefault(),
                StartTime = show.StartTime
            }).ToListAsync();

        return result.Select(show => new ShowInfo
            {
                ActGuid = actGuid,
                Venue = VenueInfo.FromEntities(show.VenueGuid, show.VenueDescription),
                StartTime = show.StartTime
            })
            .ToList();
    }

    public async Task<ShowInfo> GetShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
    {
        var result = await repository.Show
            .Where(show =>
                show.Act.ActGuid == actGuid &&
                show.Venue.VenueGuid == venueGuid &&
                show.StartTime == startTime &&
                !show.Cancelled.Any())
            .Select(show => new
            {
                VenueGuid = show.Venue.VenueGuid,
                VenueDescription = show.Venue.Descriptions
                    .OrderByDescending(d => d.ModifiedDate)
                    .FirstOrDefault()
            })
            .SingleOrDefaultAsync();

        return result == null
            ? null
            : new ShowInfo
            {
                ActGuid = actGuid,
                Venue = VenueInfo.FromEntities(result.VenueGuid, result.VenueDescription),
                StartTime = startTime
            };
    }
}