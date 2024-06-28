namespace InfoTrackSearchTracker.Utilities
{
    public static class Extension
    {
        public static bool IsURL(this string instance)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(instance, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
