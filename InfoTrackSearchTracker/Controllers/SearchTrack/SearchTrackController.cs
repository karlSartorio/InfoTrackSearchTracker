using InfoTrackSearchTracker.Models.SearchTrack;
using InfoTrackSearchTracker.Services;
using InfoTrackSearchTracker.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfoTrackSearchTracker.Controllers.SearchTrack
{
    public class SearchTrackController : Controller
    {
        ISearchTrackServices _services;
        public SearchTrackController(ISearchTrackServices services)
        {
            _services = services;
        }
        // GET: SearchTrackController
        public async Task<ActionResult> Index()
        {
            var model = await _services.GetSearchTrackViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Index(SearchTrackViewModel input)
        {
            if (input.SearchString.IsURL())
            {
                ModelState.AddModelError("SearchString", "Cannot enter a URL in a phrase");
            }
            if (!input.URLSearch.IsURL() && !string.IsNullOrWhiteSpace(input.URLSearch))
            {
                ModelState.AddModelError("URLSearch", "Please enter a valid URL to search for");
            }
            try
            {
                var model = await _services.GetCustomSearchTrack(input);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("SearchString", ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
