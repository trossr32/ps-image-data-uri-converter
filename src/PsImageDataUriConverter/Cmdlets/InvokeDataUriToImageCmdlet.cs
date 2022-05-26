using System.Management.Automation;
using PsImageDataUriConverter.Core.Converters;
using PsImageDataUriConverter.Core.Helpers;

namespace ImageDataUriConverter.Cmdlets;

/// <summary>
/// <para type="synopsis">
/// Create an image file from a base64 data uri.
/// </para>
/// <para type="description">
/// Create an image file from a base64 data uri.
/// </para>
/// <example>
///     <para>Example 1: Create an image file from a base64 data uri</para>
///     <code>PS C:\> Invoke-DataUriToImage -DataUri 'data:image/png;base64,iVBORw0K...'</code>
///     <remarks>Convert an image file to a data uri.</remarks>
/// </example>
/// <example>
///     <para>Example 2: Create an image file from a base64 data uri and specify the image file to be created</para>
///     <code>PS C:\> Invoke-DataUriToImage -DataUri 'data:image/png;base64,iVBORw0K...' -OutFile 'C:\Temp\an.image.png'</code>
///     <remarks>Create an image file from a base64 data uri and specify the image file to be created.</remarks>
/// </example>
/// <para type="link" uri="(https://github.com/trossr32/ps-image-data-uri-converter)">[Github]</para>
/// </summary>
[Cmdlet(VerbsLifecycle.Invoke, "DataUriToImage", HelpUri = "https://github.com/trossr32/ps-image-data-uri-converter")]
public class InvokeDataUriToImageCmdlet : PSCmdlet
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
    /// Output file path or file path and file name
    /// </para>
    /// </summary>
    [Parameter(Mandatory = false, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true, Position = 1)]
    public string OutFile { get; set; }
    
    /// <summary>
    /// Implements the <see cref="BeginProcessing"/> method for <see cref="InvokeImageToDataUriCmdlet"/>.
    /// </summary>
    protected override void BeginProcessing()
    {
        base.BeginProcessing();

        // ensure data uri is in the correct format
        if (!DataUri.IsDataUri())
            ThrowTerminatingError(new ErrorRecord(new Exception($"Invalid data uri supplied: {DataUri}"), null, ErrorCategory.InvalidArgument, null));
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
            var file = DataUri.CreateImageFromDataUri(OutFile);

            WriteObject($"Image created: {file}");
        }
        catch (Exception e)
        {
            ThrowTerminatingError(new ErrorRecord(new Exception($"Failed to create image with error: {e.Message}", e), null, ErrorCategory.OperationStopped, null));
        }
    }
}