using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace PsImageDataUriConverter.Core.Converters;

public static class ImageConverter
{
    /// <summary>
    /// Converts an image file to a base 64 data uri. <br />
    /// If width and/or height are provided the image is resized prior to conversion.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static string ToDataUri(this string file, int? width, int? height)
    {
        using var image = Image.Load(file, out IImageFormat format);

        if (!width.HasValue && !height.HasValue)
            return image.ToBase64String(format);

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
}