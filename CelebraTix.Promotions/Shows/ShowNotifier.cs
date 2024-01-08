using CelebraTix.Promotions.Acts;
using CelebraTix.Promotions.Data;
using Celebratix.Promotions.Messages;
using Celebratix.Promotions.Messages.Acts;
using Celebratix.Promotions.Messages.Shows;
using Celebratix.Promotions.Messages.Venues;
using CelebraTix.Promotions.Venues;
using MassTransit;

namespace CelebraTix.Promotions.Shows;

public class ShowNotifier : INotifier<Show>
{
    private readonly ActQueries actQueries;
    private readonly VenueQueries venueQueries;
    private readonly IPublishEndpoint publishEndpoint;

    public ShowNotifier(ActQueries actQueries, VenueQueries venueQueries, IPublishEndpoint publishEndpoint)
    {
        this.actQueries = actQueries;
        this.venueQueries = venueQueries;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Notify(Show show)
    {
        var act = await actQueries.GetAct(show.Act.ActGuid);
        var venue = await venueQueries.GetVenue(show.Venue.VenueGuid);
        var showAdded = new ShowAdded
        {
            act = new ActRepresentation
            {
                actGuid = act.ActGuid,
                description = new ActDescriptionRepresentation
                {
                    title = act.Title,
                    imageHash = act.ImageHash,
                    modifiedDate = new DateTime(act.LastModifiedTicks)
                }
            },
            venue = new VenueRepresentation
            {
                venueGuid = venue.VenueGuid,
                description = new VenueDescriptionRepresentation
                {
                    name = venue.Name,
                    city = venue.City,
                    modifiedDate = new DateTime(venue.LastModifiedTicks)
                },
                location = MapVenueLocation(venue),
                timeZone = new VenueTimeZoneRepresentation
                {
                    timeZone = venue.TimeZone,
                    modifiedDate = new DateTime(venue.TimeZoneLastModifiedTicks)
                }
            },
            show = new ShowRepresentation
            {
                startTime = show.StartTime
            }
        };

        await publishEndpoint.Publish(showAdded);
    }

    private static VenueLocationRepresentation MapVenueLocation(VenueInfo venue)
    {
        switch ((venue.Latitude, venue.Longitude))
        {
            case (float latitude, float longitude):
                return new VenueLocationRepresentation
                {
                    latitude = latitude,
                    longitude = longitude,
                    modifiedDate = new DateTime(venue.LocationLastModifiedTicks)
                };
            default:
                return null;
        }
    }
}

}