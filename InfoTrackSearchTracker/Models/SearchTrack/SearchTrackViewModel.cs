using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace InfoTrackSearchTracker.Models.SearchTrack
{
    public class SearchTrackViewModel
    {
        [DisplayName("Enter phrase")]
        public string SearchString { get; set; } = "";
        [DisplayName("Enter URL to search for")]
        public string? URLSearch { get; set; }
        public SearchEngine SearchEngine { get; set; } = SearchEngine.Google;
        public int NumberOfOccurrence { get; set; }
        public string? Postions { get; set; }
    }
}
