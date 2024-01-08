using Celebratix.Promotions.Messages.Acts;
using Celebratix.Promotions.Messages.Venues;

namespace Celebratix.Promotions.Messages.Shows;

public class ShowAdded
{
    public ActRepresentation act { get; set; }
    public VenueRepresentation venue { get; set; }
    public ShowRepresentation show { get; set; }
}