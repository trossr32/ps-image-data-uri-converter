# PsImageDataUriConverter

[![PowerShell Gallery Version](https://img.shields.io/powershellgallery/v/ImageDataUriConverter?label=ImageDataUriConverter&logo=powershell&style=plastic)](https://www.powershellgallery.com/packages/ImageDataUriConverter)
[![PowerShell Gallery](https://img.shields.io/powershellgallery/dt/ImageDataUriConverter?style=plastic)](https://www.powershellgallery.com/packages/ImageDataUriConverter)

A Powershell module for data URI to/from image file conversion.

Available in the [Powershell Gallery](https://www.powershellgallery.com/packages/ImageDataUriConverter)

## Description
Convert an image file to a base64 data URI, or create an image file from a base64 data URI.

Optionally resize the image prior to data URI conversion (original file retained, resizing is done in memory).

## Installation (from the Powershell Gallery)

```powershell
Install-Module ImageDataUriConverter
Import-Module ImageDataUriConverter
```

## Included cmdlets

```powershell
Invoke-ImageToDataUri
Invoke-DataUriToImage
```

## Examples

### Convert an image file to a data uri and copy to clipboard

```powershell
Invoke-ImageToDataUri -File 'C:\Temp\an.image.png' -CopyToClipboard
```

### Convert an image file to a data uri using pipeline and resize to 30px x 30px

```powershell
'C:\Temp\an.image.png' | Invoke-ImageToDataUri -Width 30 -Height 30
```

### Convert an image file to a data uri within an html img tag

```powershell
Invoke-ImageToDataUri -File 'C:\Temp\an.image.png' -AsHtmlImgTag
```

### Create an image file from a base64 data uri

```powershell
Invoke-DataUriToImage -DataUri 'data:image/png;base64,iVBORw0K...'
```

### Create an image file from a base64 data uri and specify the image file to be created

```powershell
Invoke-DataUriToImage -DataUri 'data:image/png;base64,iVBORw0K...' -OutFile 'C:\Temp\an.image.png'
```

## Building the module and importing locally

### Build the .NET core solution

```powershell
dotnet build [Github clone/download directory]\ps-image-data-uri-converter\src\PsImageDataUriConverter.csproj
```

### Copy the built files to your Powershell modules directory

Remove any existing installation in this directory, create a new module directory and copy all the built files.

```powershell
Remove-Item "C:\Users\[User]\Documents\PowerShell\Modules\ImageDataUriConverter" -Recurse -Force -ErrorAction SilentlyContinue
New-Item -Path 'C:\Users\[User]\Documents\PowerShell\Modules\ImageDataUriConverter' -ItemType Directory
Get-ChildItem -Path "[Github clone/download directory]\ps-image-data-uri-converter\src\PsImageDataUriConverterCmdlet\bin\Debug\net6.0\" | Copy-Item -Destination "C:\Users\[User]\Documents\PowerShell\Modules\ImageDataUriConverter" -Recurse
```

## Contribute

Please raise an issue if you find a bug or want to request a new feature, or create a pull request to contribute.

<a href='https://ko-fi.com/K3K22CEIT' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://cdn.ko-fi.com/cdn/kofi4.png?v=2' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>
