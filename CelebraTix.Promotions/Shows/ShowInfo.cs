using CelebraTix.Promotions.Venues;

namespace CelebraTix.Promotions.Shows;

public class ShowInfo
{
    public Guid ActGuid { get; set; }
    public VenueInfo Venue { get; set; }
    public DateTimeOffset StartTime { get; set; }
}