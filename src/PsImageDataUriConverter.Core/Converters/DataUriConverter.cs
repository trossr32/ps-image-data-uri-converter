using PsImageDataUriConverter.Core.Helpers;
using RegExtract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PsImageDataUriConverter.Core.Converters;

public static class DataUriConverter
{
    /// <summary>
    /// Takes a data uri and creates an image file.
    /// </summary>
    /// <param name="dataUri">The string to convert.</param>
    /// <param name="file">The file path to save the image to.</param>
    /// <returns>The image object.</returns>
    public static string CreateImageFromDataUri(this string dataUri, string file)
    {
        var bytes = dataUri.AsByteArray();
        var extension = dataUri.ExtractImageType().AsImageTypeExtension();
        
        var image = string.IsNullOrWhiteSpace(file) 
            ? Path.Combine(Path.GetTempPath(), extension.AsTempFileName()) 
            : Directory.Exists(file) 
                ? Path.Combine(file, extension.AsTempFileName()) 
                : file;

        using var fs = File.Create(image);
        fs.Write(bytes);

        return image;
    }

    /// <summary>
    /// Converts a data uri to an image, resizes, then converts back to a data uri
    /// </summary>
    /// <param name="dataUri"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static string Resize(this string dataUri, int? width, int? height)
    {
        Image.Identify(dataUri.AsByteArray(), out var format);

        var image = Image.Load(dataUri.AsByteArray());

        var w = width ?? image.Width;
        var h = height ?? image.Height;

        image.Mutate(i => i.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Position = AnchorPositionMode.Center,
            Size = new Size(w, h)
        }));
        
        return image.ToBase64String(format);
    }

    /// <summary>
    /// Convert a data uri into a byte array
    /// </summary>
    /// <param name="dataUri"></param>
    /// <returns></returns>
    private static byte[] AsByteArray(this string dataUri) =>
        Convert.FromBase64String(dataUri.ExtractBase64());

    /// <summary>
    /// Get a temp file name using the supplied file extension which should be prefixed with a period, e.g .png
    /// </summary>
    /// <param name="extension"></param>
    /// <returns></returns>
    private static string AsTempFileName(this string extension) =>
        $"ps.datauri.conversion.{DateTime.Now:yyyyMMdd.HHmmss}{extension}";

    /// <summary>
    /// Gets an image type from a data URI, e.g. data:image/png;base64,iVBORw0K... gives 'png'
    /// </summary>
    /// <param name="dataUri"></param>
    /// <returns></returns>
    private static string ExtractImageType(this string dataUri) =>
        dataUri.Extract<string>(@"data:image\/([a-zA-Z]+);base64,");

    /// <summary>
    /// Gets an image type from a data URI, e.g. data:image/png;base64,iVBORw0K... gives 'png'
    /// </summary>
    /// <param name="dataUri"></param>
    /// <returns></returns>
    private static string ExtractBase64(this string dataUri) =>
        dataUri.Extract<string>(@"data:image\/[a-zA-Z]+;base64,(.+)");
}