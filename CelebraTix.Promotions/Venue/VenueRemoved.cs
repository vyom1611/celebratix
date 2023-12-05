namespace TicketerApplication.Venue;

public class VenueRemoved
{
    public int VenueRemovedId { get; set; }

    public Venue Venue { get; set; }
    
    public int VenueId { get; set; }
    
    public DateTime RemovedDate { get; set; }
}