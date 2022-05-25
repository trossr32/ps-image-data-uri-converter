namespace PsImageDataUriConverter.Core.Helpers;

public static class ImageTypeHelpers
{
    public static readonly Dictionary<string, string> SupportedImageFormats = new()
    {
        {".jpg", "jpeg"},
        {".jpeg", "jpeg"},
        {".jfif", "jpeg"},
        {".pjp", "jpeg"},
        {".pjpeg", "jpeg"},
        {".png", "png"},
        {".svg", "svg+xml"},
        {".webp", "webp"},
        {".gif", "gif"},
        {".avif", "avif"},
        {".apng", "apng"},
        {".ico", "x-icon"},
        {".cur", "x-icon"},
        {".tiff", "tiff"},
        {".tif", "tiff"},
    };

    //public static readonly Dictionary<string, string> DataUriImageTypeMappings = new()
    //{
    //    {"jpeg", ".jpg"},
    //    {"png", ".png"},
    //    {"svg+xml", ".svg"},
    //    {"webp", ".webp"},
    //    {"gif", ".gif"},
    //    {"avif", ".avif"},
    //    {"apng", ".apng"},
    //    {"x-icon", ".ico"},
    //    {"tiff", ".tif"}
    //};

    public static string AsImageTypeExtension(this string dataUriFormatType) =>
        SupportedImageFormats.First(kvp => kvp.Value == dataUriFormatType).Key;

    public static string AsDataUriImageType(this string imageTypeExtension) =>
        SupportedImageFormats[imageTypeExtension];
}