using System.Text.RegularExpressions;

namespace PsImageDataUriConverter.Core.Helpers;

public static class DataUriHelpers
{
    /// <summary>
    /// Regex check to see if an image URL is a base64 encoded string, e.g. data:image/png;base64,iVBORw0K...
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static bool IsDataUri(this string uri) =>
        Regex.IsMatch(uri, @"data:image\/[a-zA-Z]+;base64,.+");
}