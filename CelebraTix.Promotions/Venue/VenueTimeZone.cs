using System.ComponentModel.DataAnnotations;

namespace TicketerApplication.Venue;

public class VenueTimeZone
{
    public int VenueTimeZoneId { get; set; }

    public int VenueId { get; set; }
    
    public Venue Venue { get; set; }

    [Required]
    [MaxLength(50)]
    public string TimeZone { get; set; }

    public DateTime ModifiedDate { get; set; }
}