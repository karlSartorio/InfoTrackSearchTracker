using InfoTrackSearchTracker.Models;
using InfoTrackSearchTracker.Models.SearchTrack;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InfoTrackSearchTracker.Services
{
    public class SearchTrackServices : ISearchTrackServices
    {
        public const string defaultSearchURL = "https://www.google.co.uk/search?num=100&q=land+registry+search";
        private const string defultSiteToTrack = "https://www.infotrack.co.uk/";
        private const string defaultSearchTitle = "land registry search";
        public async Task<SearchTrackViewModel> GetSearchTrackViewModel()
        {
            var result = new SearchTrackViewModel();
            var ResultPosition = await PerformAsyncSearchTrack(defaultSearchURL, defultSiteToTrack);
            result.SearchString = defaultSearchTitle;
            result.URLSearch = defultSiteToTrack;
            result.NumberOfOccurrence = ResultPosition.Count();
            if (ResultPosition.Count > 0)
            {
                result.Postions = string.Join(", ", ResultPosition);
            }
            else
            {
                result.Postions = "0";
            }
            return result;
        }
        public async Task<SearchTrackViewModel> GetCustomSearchTrack(SearchTrackViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.URLSearch))
            {
                model.URLSearch = defultSiteToTrack;
            }
            var searchURL = ConvertPhraseToSearchUrl(model.SearchEngine, model.SearchString);
            var ResultPosition = await PerformAsyncSearchTrack(searchURL, model.URLSearch, model.SearchEngine);
            model.NumberOfOccurrence = ResultPosition.Count();
            if (ResultPosition.Count > 0)
            {
                model.Postions = string.Join(", ", ResultPosition);
            }
            else
            {
                model.Postions = "0";
            }
            return model;
        }
        private async Task<List<int>> PerformAsyncSearchTrack(string searchURL, string siteToTrack, SearchEngine searchEngine = SearchEngine.Google)
        {
            List<int> result = new List<int>();
            try
            {
                // get result web page
                string htmlContent = await FetchResultPageContent(searchURL);
                // get postions of desired websites 
                result = GetWebsitePositions(htmlContent, siteToTrack, searchEngine);
                return result;
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
        }

        private List<int> GetWebsitePositions(string htmlContent, string siteToTrack, SearchEngine searchEngine)
        {
            List<int> positions = new List<int>();
            int position = 0;

            string pattern = GetSearchEngineUrlResultPattern(searchEngine);
            MatchCollection matches = Regex.Matches(htmlContent, pattern);

            foreach (Match match in matches)
            {
                position++;
                string url = Uri.UnescapeDataString(match.Groups[1].Value);

                // Check if the URL contains the company's website domain
                if (url.Contains(siteToTrack))
                {
                    positions.Add(position);
                }
            }
            return positions;
        }
        private async Task<string> FetchResultPageContent(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string htmlContent = await response.Content.ReadAsStringAsync();
                return htmlContent;
            }
        }
        private string GetSearchEngineUrlResultPattern(SearchEngine selectedSearchEngine)
        {
            string pattern = "";
            switch (selectedSearchEngine)
            {
                case SearchEngine.Bing:
                    pattern = @"<a\s+href=""(https?://[^""]+)""";
                    break;
                case SearchEngine.Google:
                default:
                    pattern = @"<a href=""/url\?q=(https?:\/\/[^&]*)";
                    break;
            }
            return pattern;
        }
        private string ConvertPhraseToSearchUrl(SearchEngine selectedSearchEngine, string searchPhrase)
        {
            string encodedSearchPhrase = WebUtility.UrlEncode(searchPhrase);
            string searchUrl = "";
            switch (selectedSearchEngine)
            {
                case SearchEngine.Bing:
                    searchUrl = $"https://www.bing.com/search?num=100&q={encodedSearchPhrase}";
                    break;
                case SearchEngine.Google:
                default:
                    searchUrl = $"https://www.google.com/search?num=100&q={encodedSearchPhrase}";
                    break;
            }
            return searchUrl;
        }
    }
}
