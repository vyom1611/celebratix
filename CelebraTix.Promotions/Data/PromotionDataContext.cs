using CelebraTix.Promotions.Acts;
using CelebraTix.Promotions.Shows;
using CelebraTix.Promotions.Venues;
using Microsoft.EntityFrameworkCore;

namespace CelebraTix.Promotions.Data;

public class PromotionDataContext : DbContext
{
    public PromotionDataContext(DbContextOptions<PromotionDataContext> options) : base(options)
    {
    }

    public DbSet<Act> Act { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<VenueDescription> VenueDescription { get; set; }
    public DbSet<VenueLocation> VenueLocation { get; set; }
    public DbSet<VenueTimeZone> VenueTimeZone { get; set; }
    public DbSet<Show> Show { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Act>()
            .HasAlternateKey(act => new { act.ActGuid });
        
        modelBuilder.Entity<Venue>()
            .HasAlternateKey(venue => new { venue.VenueGuid });

        modelBuilder.Entity<VenueDescription>()
            .HasAlternateKey(venueDescription => new { venueDescription.VenueId, venueDescription.ModifiedDate });

        modelBuilder.Entity<VenueLocation>()
            .HasAlternateKey(venueLocation => new { venueLocation.VenueId, venueLocation.ModifiedDate });

        modelBuilder.Entity<VenueTimeZone>()
            .HasAlternateKey(venueTimeZone => new { venueTimeZone.VenueId, venueTimeZone.ModifiedDate });
        
        modelBuilder.Entity<Show>()
            .HasAlternateKey(show => new { show.ActId, show.VenueId, show.StartTime });

        modelBuilder.Entity<ShowCancelled>()
            .HasAlternateKey(showCancelled => new { showCancelled.ShowId, showCancelled.CancelledDate });

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
    
    public async Task<Act> GetOrInsertAct(Guid actGuid)
    {
        var act = await Act
            .Include(a => a.Descriptions)
            .FirstOrDefaultAsync(a => a.ActGuid == actGuid);

        if (act == null)
        {
            act = new Act { ActGuid = actGuid };
            Act.Add(act);
            // You might choose to call SaveChangesAsync here or leave it to be called explicitly outside this method
        }

        return act;
    }

    public async Task<Show> GetOrInsertShow(Guid actGuid, Guid venueGuid, DateTimeOffset startTime)
    {
        var show = Show
            .Where(show =>
                show.Act.ActGuid == actGuid &&
                show.Venue.VenueGuid == venueGuid &&
                show.StartTime == startTime)
            .SingleOrDefault();
        if (show == null)
        {
            show = new Show
            {
                Act = await GetOrInsertAct(actGuid),
                Venue = await GetOrInsertVenue(venueGuid),
                StartTime = startTime
            };
            await base.AddAsync(show);
        }

        return show;
    }
}