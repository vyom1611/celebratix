using CelebraTix.Promotions.Acts;
using CelebraTix.Promotions.Venues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CelebraTix.Promotions.Shows
{
    public class ShowController : Controller
    {
        private readonly ShowCommands showCommands;
        private readonly ShowQueries showQueries;
        private readonly VenueQueries venueQueries;
        private readonly ActQueries actQueries;

        public ShowController(ShowCommands showCommands, ShowQueries showQueries, VenueQueries venueQueries,
            ActQueries actQueries)
        {
            this.showCommands = showCommands;
            this.showQueries = showQueries;
            this.venueQueries = venueQueries;
            this.actQueries = actQueries;
        }

        public async Task<IActionResult> Create(Guid id)
        {
            var act = await actQueries.GetAct(id);
            if (act == null)
            {
                return NotFound();
            }

            var viewModel = await BuildCreateShowViewModel(act);
            CheckForCustomErrors();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("Venue,StartTime")] CreateShowViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var validationError = await ValidateVenueAndGetTimeZoneError(viewModel.Venue);
            if (!string.IsNullOrEmpty(validationError))
            {
                TempData["CustomError"] = validationError;
                return RedirectToAction(nameof(Create), new { id });
            }

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById((await venueQueries.GetVenue(viewModel.Venue)).TimeZone);
            var offset = timeZone.GetUtcOffset(viewModel.StartTime);
            await showCommands.ScheduleShow(id, viewModel.Venue, new DateTimeOffset(viewModel.StartTime, offset));
            return RedirectToAction("Details", "Act", new { id });
        }

        [HttpGet("Shows/Delete/{act}/{venue}/{starttime}", Name = "DeleteShow")]
        public async Task<IActionResult> Delete(Guid act, Guid venue, DateTimeOffset starttime)
        {
            var show = await showQueries.GetShow(act, venue, starttime);
            return show == null ? NotFound() : View(show);
        }

        [HttpPost("Shows/Delete/{act}/{venue}/{starttime}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid act, Guid venue, DateTimeOffset starttime)
        {
            await showCommands.CancelShow(act, venue, starttime);
            return RedirectToAction("Details", "Act", new { id = act });
        }

        private async Task<CreateShowViewModel> BuildCreateShowViewModel(ActInfo act)
        {
            var venues = await venueQueries.ListVenues();
            return new CreateShowViewModel
            {
                Act = act,
                Venues = venues.Select(venue => new SelectListItem
                {
                    Value = venue.VenueGuid.ToString(),
                    Text = $"{venue.Name}, {venue.City}"
                }).ToList(),
                StartTime = DateTime.Now.Date.AddDays(7).AddHours(19)
            };
        }

        private void CheckForCustomErrors()
        {
            if (TempData["CustomError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
            }
        }

        private async Task<string> ValidateVenueAndGetTimeZoneError(Guid venueGuid)
        {
            var venue = await venueQueries.GetVenue(venueGuid);
            if (venue == null)
            {
                return "Venue not found";
            }

            if (venue.TimeZone == null)
            {
                return "The selected venue does not have a time zone";
            }

            if (TimeZoneInfo.FindSystemTimeZoneById(venue.TimeZone) == null)
            {
                return $"The selected venue has an invalid time zone: {venue.TimeZone}";
            }

            return null;
        }
    }
}
