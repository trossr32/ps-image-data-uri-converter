using System.Management.Automation;
using PsImageDataUriConverter.Core.Converters;
using PsImageDataUriConverter.Core.Helpers;
using TextCopy;

namespace ImageDataUriConverter.Cmdlets;

/// <summary>
/// <para type="synopsis">
/// Convert a base64 data uri to a resized data uri.
/// </para>
/// <para type="description">
/// Convert a base64 data uri to a resized data uri.
/// </para>
/// <example>
///     <para>Example 1: Resize a data uri to 30px x 30px and copy to clipboard</para>
///     <code>PS C:\> Invoke-DataUriResize -DataUri 'data:image/png;base64,iVBORw0K...' -Width 30 -Height 30 -CopyToClipboard</code>
///     <remarks>Resize a data uri to 30px x 30px and copy to clipboard.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-image-data-uri-converter)">[Github]</para>
/// </summary>
[Cmdlet(VerbsLifecycle.Invoke, "DataUriResize", HelpUri = "https://github.com/trossr32/ps-image-data-uri-converter")]
public class InvokeDataUriResizeCmdlet : PSCmdlet
{
    /// <summary>
    /// <para type="description">
    /// Data uri, e.g. data:image/png;base64,iVBORw0K...
    /// </para>
    /// </summary>
    [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Position = 0)]
    public string DataUri { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied then the image will be resized to this width maintaining aspect ratio.
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, Position = 1)]
    public int? Width { get; set; }

    /// <summary>
    /// <para type="description">
    /// If supplied then the image will be resized to this height maintaining aspect ratio.
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
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="InvokeDataUriResizeCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        // ensure data uri is in the correct format
        if (!DataUri.IsDataUri())
            ThrowTerminatingError(new ErrorRecord(new Exception($"Invalid data uri supplied: {DataUri}"), null, ErrorCategory.InvalidArgument, null));
    }

    /// <summary>
    /// Implements the <see cref="ProcessRecord"/> method for <see cref="InvokeDataUriResizeCmdlet"/>.
    /// </summary>
    protected override void ProcessRecord()
    {

    }

    /// <summary>
    /// Implements the <see cref="EndProcessing"/> method for <see cref="InvokeDataUriResizeCmdlet"/>.
    /// Retrieve all torrents
    /// </summary>
    protected override void EndProcessing()
    {
        try
        {
            var dataUri = DataUri.Resize(Width, Height);

            if (CopyToClipboard.IsPresent)
                ClipboardService.SetText(dataUri);

            WriteObject(dataUri);
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to resize data uri with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}