using Microsoft.AspNetCore.Mvc;

namespace CelebraTix.Promotions.Acts;

public class ActController : Controller
{
    private readonly ActCommands actCommands;
        private readonly ActQueries actQueries;
        private readonly ContentCommands contentCommands;
        private readonly ShowQueries showQueries;

        public ActsController(ActCommands actCommands, ActQueries actQueries, ContentCommands contentCommands, ShowQueries showQueries)
        {
            this.actCommands = actCommands ?? throw new ArgumentNullException(nameof(actCommands));
            this.actQueries = actQueries ?? throw new ArgumentNullException(nameof(actQueries));
            this.contentCommands = contentCommands ?? throw new ArgumentNullException(nameof(contentCommands));
            this.showQueries = showQueries ?? throw new ArgumentNullException(nameof(showQueries));
        }

        public async Task<IActionResult> Index()
        {
            var acts = await actQueries.ListActs();
            return View(acts);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var act = await actQueries.GetAct(id);
            if (act == null)
            {
                return NotFound();
            }

            var shows = await showQueries.ListShows(id);
            var viewModel = new ActViewModel { Act = act, Shows = shows };

            return View(viewModel);
        }

        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Create), new { id = Guid.NewGuid() });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("Title,Image,ImageHash")] ActInfo act)
        {
            act.ImageHash = await ProcessImageAndGetHash(act);

            if (ModelState.IsValid)
            {
                act.ActGuid = id;
                await actCommands.SaveAct(act);
                return RedirectToAction(nameof(Index));
            }

            return View(act);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var actInfo = await actQueries.GetAct(id);
            if (actInfo == null)
            {
                return NotFound();
            }

            return View(actInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,Image,ImageHash,LastModifiedTicks")] ActInfo act)
        {
            act.ImageHash = await ProcessImageAndGetHash(act);

            if (ModelState.IsValid)
            {
                act.ActGuid = id;
                await actCommands.SaveAct(act);
                return RedirectToAction(nameof(Index));
            }

            return View(act);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var actInfo = await actQueries.GetAct(id);
            if (actInfo == null)
            {
                return NotFound();
            }

            return View(actInfo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await actCommands.RemoveAct(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> ProcessImageAndGetHash(ActInfo act)
        {
            if (act.Image != null)
            {
                using var imageMemoryStream = new MemoryStream();
                await act.Image.OpenReadStream().CopyToAsync(imageMemoryStream);
                return await contentCommands.SaveContent(imageMemoryStream.ToArray(), act.Image.ContentType);
            }

            return act.ImageHash;
        }
}