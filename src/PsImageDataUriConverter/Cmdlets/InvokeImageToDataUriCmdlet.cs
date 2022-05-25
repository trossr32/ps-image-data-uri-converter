using System.Management.Automation;
using PsImageDataUriConverter.Core.Converters;
using PsImageDataUriConverter.Core.Helpers;
using TextCopy;

namespace ImageDataUriConverter.Cmdlets;

/// <summary>
/// <para type="synopsis">
/// Convert an image file to a base64 data uri.
/// </para>
/// <para type="description">
/// Convert an image file to a base64 data uri.
/// </para>
/// <para type="description">
/// Optionally resize the image first by supplying width and/or height parameters, maintaining aspect ratio (original file retained, resizing is done in memory).
/// </para>
/// <example>
///     <para>Example 1: Convert an image file to a data uri and copy to clipboard</para>
///     <code>PS C:\> Invoke-ImageToDataUri -File 'C:\Temp\an.image.png' -CopyToClipboard</code>
///     <remarks>Convert an image file to a data uri and copy to clipboard.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Convert an image file to a data uri using pipeline and resize to 30px x 30px</para>
///     <code>PS C:\> 'C:\Temp\an.image.png' | Invoke-ImageToDataUri -Width 30 -Height 30</code>
///     <remarks>Convert an image file to a data uri using pipeline and resize to 30px x 30px.</remarks>
/// </example>
/// <example>
///     <para>Example 3: Convert an image file to a data uri within an html img tag</para>
///     <code>PS C:\> Invoke-ImageToDataUri -File 'C:\Temp\an.image.png' -AsHtmlImgTag</code>
///     <remarks>Convert an image file to a data uri within an html img tag.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-image-data-uri-converter)">[Github]</para>
/// </summary>
[Cmdlet(VerbsLifecycle.Invoke, "ImageToDataUri", HelpUri = "https://github.com/trossr32/ps-image-data-uri-converter")]
public class InvokeImageToDataUriCmdlet : PSCmdlet
{
    /// <summary>
    /// <para type="description">
    /// Image file name relative to current location, or relative path and image file name, or full path of image
    /// </para>
    /// </summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
    public string Image { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied then the image will be resized to this width maintaining aspect ratio. (original file retained, resizing is done in memory)
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, Position = 1)]
    public int? Width { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied then the image will be resized to this height maintaining aspect ratio. (original file retained, resizing is done in memory)
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, Position = 2)]
    public int? Height { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied the data uri will be saved to the clipboard.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter CopyToClipboard { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied the data uri will be returned embedded in an img html tag.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false)]
    public SwitchParameter AsHtmlImgTag { get; set; }

    private string _image;

    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="InvokeImageToDataUriCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        // ensure image path is correct
        _image = File.Exists(Image)
            ? Image
            : Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Image.TrimStart('.', '/'));

        if (!File.Exists(_image))
            ThrowTerminatingError(new ErrorRecord(new Exception($"Unable to find supplied image file: {_image}"), null, ErrorCategory.InvalidArgument, null));

        // check format
        var extension = Path.GetExtension(_image);

        if (!ImageTypeHelpers.SupportedImageFormats.TryGetValue(extension, out _))
            ThrowTerminatingError(new ErrorRecord(new Exception($"The supplied image file is of an unsupported type ({extension}). Supported file extensions are: {ImageTypeHelpers.SupportedImageFormats.Keys.Aggregate((a, b) => $"{a}, {b}")}"), null, ErrorCategory.InvalidArgument, null));
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="InvokeImageToDataUriCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {

    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="InvokeImageToDataUriCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
        try
        {
            var dataUri = AsHtmlImgTag.IsPresent
                ? $"<img src=\"{_image.ToDataUri(Width, Height)}\" />"
                : _image.ToDataUri(Width, Height);

            if (CopyToClipboard.IsPresent)
                 ClipboardService.SetText(dataUri);

            WriteObject(dataUri);
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to convert image with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}