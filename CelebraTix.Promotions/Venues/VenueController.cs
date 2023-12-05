using CelebraTix.Promotions.Venues;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CelebraTix.Promotions.Venues;

/// <summary>
/// Controller for managing venues in the CelebraTix application.
/// Provides actions for listing, creating, editing, and deleting venues.
/// </summary>
public class VenuesController : Controller
{
    private readonly VenueQueries queries;
    private readonly VenueCommands commands;

    /// <summary>
    /// Constructor for VenuesController.
    /// </summary>
    /// <param name="queries">Service for querying venue data.</param>
    /// <param name="commands">Service for executing commands on venue data.</param>
    public VenuesController(VenueQueries queries, VenueCommands commands)
    {
        this.queries = queries;
        this.commands = commands;
    }

    /// <summary>
    /// Displays a list of all venues.
    /// </summary>
    /// <returns>A view displaying a list of venues.</returns>
    public async Task<IActionResult> Index()
    {
        var venues = await queries.ListVenues();
        return View(venues);
    }

    /// <summary>
    /// Displays details of a specific venue.
    /// </summary>
    /// <param name="id">The unique identifier of the venue.</param>
    /// <returns>A view showing the details of a venue.</returns>
    public async Task<IActionResult> Details(Guid id)
    {
        var venue = await queries.GetVenue(id);
        if (venue == null)
        {
            return NotFound();
        }

        return View(venue);
    }

    /// <summary>
    /// Displays a form for creating a new venue.
    /// Redirects to a filled form if an id is provided.
    /// </summary>
    /// <param name="id">Optional unique identifier for pre-filling the form (if exists).</param>
    /// <returns>A view containing the form for creating a venue.</returns>
    public IActionResult Create(Guid? id)
    {
        if (id == null)
        {
            return View(new VenueInfo());
        }
        else
        {
            return RedirectToAction(nameof(Create), new { id = Guid.NewGuid() });
        }
    }

    /// <summary>
    /// Handles the submission of a new venue.
    /// </summary>
    /// <param name="id">The unique identifier for the new venue.</param>
    /// <param name="venue">The venue data bound from the form submission.</param>
    /// <returns>Redirects to the index view on success, or redisplays the form on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid id, [Bind("Name,City")] VenueInfo venue)
    {
        if (ModelState.IsValid)
        {
            venue.VenueGuid = id;
            await commands.SaveVenue(venue);
            return RedirectToAction(nameof(Index));
        }

        return View(venue);
    }

    /// <summary>
    /// Displays a form for editing an existing venue.
    /// </summary>
    /// <param name="id">The unique identifier of the venue to edit.</param>
    /// <returns>A view containing the form for editing a venue.</returns>
    public async Task<IActionResult> Edit(Guid id)
    {
        var venue = await queries.GetVenue(id);
        if (venue == null)
        {
            return NotFound();
        }

        return View(venue);
    }

    /// <summary>
    /// Handles the submission of venue edits.
    /// </summary>
    /// <param name="id">The unique identifier of the venue being edited.</param>
    /// <param name="venue">The venue data bound from the form submission.</param>
    /// <returns>Redirects to the index view on success, or redisplays the form on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Name,City")] VenueInfo venue)
    {
        if (ModelState.IsValid)
        {
            venue.VenueGuid = id;
            await commands.SaveVenue(venue);
            return RedirectToAction(nameof(Index));
        }

        return View(venue);
    }

    /// <summary>
    /// Displays a confirmation page for venue deletion.
    /// </summary>
    /// <param name="id">The unique identifier of the venue to delete.</param>
    /// <returns>A view asking for confirmation to delete a venue.</returns>
    public async Task<IActionResult> Delete(Guid id)
    {
        var venue = await queries.GetVenue(id);
        if (venue == null)
        {
            return NotFound();
        }

        return View(venue);
    }

    /// <summary>
    /// Handles the confirmed deletion of a venue.
    /// </summary>
    /// <param name="id">The unique identifier of the venue to delete.</param>
    /// <returns>Redirects to the index view on successful deletion.</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await commands.DeleteVenue(id);
        return RedirectToAction(nameof(Index));
    }
}