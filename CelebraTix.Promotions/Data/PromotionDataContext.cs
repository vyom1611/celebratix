using CelebraTix.Promotions.Venues;
using Microsoft.EntityFrameworkCore;

namespace CelebraTix.Promotions.Data;

public class PromotionDataContext : DbContext
{
    public PromotionDataContext(DbContextOptions<PromotionDataContext> options) : base(options)
    {
    }

    public DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Venues.Venue>().HasKey(v => v.VenueGuid);
    }

    public async Task<Venue> GetOrInsertVenue(Guid venueGuid)
    {
        var venue = await Venues.FirstOrDefaultAsync(v => v.VenueGuid == venueGuid);

        if (venue == null)
        {
            venue = new Venue { VenueGuid = venueGuid };
            await Venues.AddAsync(venue);
        }

        return venue;
    }
}