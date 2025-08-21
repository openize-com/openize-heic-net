# Openize.IsoBmff.ContentLightLevelInfo

Contains light level information about the image.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ToString** | **string** | Text summary of the box. | 

## Fields

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**max_content_light_level** | **ushort** | Indicates the max picture ligth level. | 
**max_pic_average_light_level** | **ushort** | Indicates the picture average ligth level. | 

## Constructors

Name | Description | Parameters
------------ | ------------- | ------------- 
**ContentLightLevelInfo** | Create the box object from the bitstream and box size. | BitStreamReader <b>stream</b> - File stream.<br />ulong <b>size</b> - Box size in bytes.

[[Back to API_README]](API_README.md)