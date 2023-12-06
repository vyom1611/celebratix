using Microsoft.AspNetCore.Mvc.Rendering;

namespace CelebraTix.Promotions.Venues;

public class VenueViewModel
{
    public VenueInfo Venue { get; set; }
    public List<SelectListItem> TimeZones { get; set; }
}