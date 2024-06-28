using InfoTrackSearchTracker.Models;
using InfoTrackSearchTracker.Models.SearchTrack;

namespace InfoTrackSearchTracker.Services
{
    public interface ISearchTrackServices
    {
        Task<SearchTrackViewModel> GetSearchTrackViewModel();
        Task<SearchTrackViewModel> GetCustomSearchTrack(SearchTrackViewModel model);
    }
}
