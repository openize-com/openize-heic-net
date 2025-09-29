# Openize.Heic.Decoder.ExifData

Exchangeable image file format class.
Grants access to raw exif-data and to data by specific tags.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**DirectoriesList** | **List<MetadataExtractor.Directory>** | List of exif directories in frame. | 
**TagList** | **List<MetadataExtractor.Tag>** | List of all tags that present in directories. | 
**RawBytes** | **byte[]** | Raw exif data in bytes. | 

## Constructors

Name | Description | Parameters
------------ | ------------- | -------------
**ExifData** | Create exif-data object. | BitStreamWithNalSupport <b>stream</b> - File stream.<br />HeicImage <b>image</b> - Parent heic image.<br />HeicImageFrame <b>exifFrame</b> - Exif heic frame.

## Methods

Name | Type | Description | Parameters
------------ | ------------- | ------------- | -------------
**GetExifString<T>** | **string** | Returns the string value for the particular tag type and the directorty specified as a class. | int <b>tagType</b> - The tag type identifier. | 
**GetExifString<T>** | **object** | Returns the raw value for the particular tag type and the directorty specified as a class. | int <b>tagType</b> - The tag type identifier. | 
**GetExifString** | **string** | Returns the string value for the particular tag type and the directorty specified as a parameter. | ExifDirectoryType <b>dirType</b> - The directory type identifier.<br />int <b>tagType</b> - The tag type identifier. | 
**GetExifString** | **object** | Returns the string value for the particular tag type and the directorty specified as a parameter. | ExifDirectoryType <b>dirType</b> - The directory type identifier.<br />int <b>tagType</b> - The tag type identifier. | 

[[Back to API_README]](API_README.md)