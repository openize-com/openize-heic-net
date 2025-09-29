/*
 * Openize.HEIC 
 * Copyright (c) 2024-2025 Openize Pty Ltd. 
 *
 * This file is part of Openize.HEIC.
 *
 * Openize.HEIC is available under Openize license, which is
 * available along with Openize.HEIC sources.
 */

namespace Openize.Heic.Decoder
{
    /// <summary>
    /// Type of an exif directory.
    /// </summary>
    public enum ExifDirectoryType
    {
        /// <summary>
        /// IFD0 directory.
        /// </summary>
        ExifIfd0Directory,

        /// <summary>
        /// One of several Exif directories.
        /// Holds information about image IFD's in a chain after the first.
        /// </summary>
        ExifImageDirectory,

        /// <summary>
        /// Exif interoperability tags directory.
        /// </summary>
        ExifInteropDirectory,

        /// <summary>
        /// SubIFD directory.
        /// </summary>
        ExifSubIfdDirectory,

        /// <summary>
        /// IFD1 directory. Holds information about an embedded thumbnail image.
        /// </summary>
        ExifThumbnailDirectory,

        /// <summary>
        /// GPS Exif tags directory.
        /// </summary>
        GpsDirectory,

        /// <summary>
        /// Directory for image encoding information for DCT filters, as stored by Adobe.
        /// </summary>
        AdobeJpegDirectory,

        /// <summary>
        /// Directory for tags specific to Apple cameras.
        /// </summary>
        AppleMakernoteDirectory,

        /// <summary>
        /// Directory for basic metadata for Avi files.
        /// </summary>
        AviDirectory,

        /// <summary>
        /// Directory for basic metadata for Bmp files.
        /// </summary>
        BmpHeaderDirectory,

        /// <summary>
        /// Directory for tags specific to Canon cameras.
        /// </summary>
        CanonMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Casio (type 1) cameras.
        /// </summary>
        CasioType1MakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Casio (type 2) cameras.
        /// </summary>
        CasioType2MakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to DJI aircraft cameras.
        /// </summary>
        DjiMakernoteDirectory,

        /// <summary>
        /// Directory for tags from Photoshop "ducky" segments.
        /// </summary>
        DuckyDirectory,

        /// <summary>
        /// Directory for basic metadata for Eps files.
        /// </summary>
        EpsDirectory,

        /// <summary>
        /// Directory for the reporting of errors.
        /// </summary>
        ErrorDirectory,

        /// <summary>
        /// Directory for OS tags.
        /// </summary>
        FileMetadataDirectory,

        /// <summary>
        /// Directory for tags derived from filename.
        /// </summary>
        FileTypeDirectory,

        /// <summary>
        /// Directory for camera info tags specific to FLIR cameras.
        /// </summary>
        FlirCameraInfoDirectory,

        /// <summary>
        /// Directory for header tags specific to FLIR cameras.
        /// </summary>
        FlirHeaderDirectory,

        /// <summary>
        /// Directory for tags specific to FLIR cameras.
        /// </summary>
        FlirMakernoteDirectory,

        /// <summary>
        /// Directory for raw tags specific to FLIR cameras.
        /// </summary>
        FlirRawDataDirectory,

        /// <summary>
        /// Directory for tags specific to Fujifilm cameras.
        /// </summary>
        FujifilmMakernoteDirectory,

        /// <summary>
        /// Directory for tiff geo tags.
        /// </summary>
        GeoTiffDirectory,

        /// <summary>
        /// Directory for animation metadata for Gif files.
        /// </summary>
        GifAnimationDirectory,

        /// <summary>
        /// Directory for comment metadata for Gif files.
        /// </summary>
        GifCommentDirectory,

        /// <summary>
        /// Directory for control metadata for Gif files.
        /// </summary>
        GifControlDirectory,

        /// <summary>
        /// Directory for header metadata for Gif files.
        /// </summary>
        GifHeaderDirectory,

        /// <summary>
        /// Directory for basic metadata for Gif files.
        /// </summary>
        GifImageDirectory,

        /// <summary>
        /// Directory for basic metadata for Heic files.
        /// </summary>
        HeicImagePropertiesDirectory,

        /// <summary>
        /// Directory for basic metadata for Heic thumbnails.
        /// </summary>
        HeicThumbnailDirectory,

        /// <summary>
        /// Directory of tables for the DHT (Define Huffman Table(s)) segment.
        /// </summary>
        HuffmanTablesDirectory,

        /// <summary>
        /// Directory for basic metadata for Icc files.
        /// </summary>
        IccDirectory,

        /// <summary>
        /// Directory for basic metadata for Ico files.
        /// </summary>
        IcoDirectory,

        /// <summary>
        /// Directory for tags used by the International Press Telecommunications Council (IPTC) metadata format.
        /// </summary>
        IptcDirectory,

        /// <summary>
        /// Directory for basic metadata for Jfif files.
        /// </summary>
        JfifDirectory,

        /// <summary>
        /// Directory for basic metadata for Jfxx files.
        /// </summary>
        JfxxDirectory,

        /// <summary>
        /// Directory for comment metadata for Jpeg files.
        /// </summary>
        JpegCommentDirectory,

        /// <summary>
        /// Directory for basic metadata for Jpeg files.
        /// </summary>
        JpegDirectory,

        /// <summary>
        /// Directory for DNL metadata for Jpeg files.
        /// </summary>
        JpegDnlDirectory,

        /// <summary>
        /// Directory for tags specific to Kodak cameras.
        /// </summary>
        KodakMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Kyocera cameras.
        /// </summary>
        KyoceraMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Leica cameras.
        /// </summary>
        LeicaMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Leica (type 5) cameras.
        /// </summary>
        LeicaType5MakernoteDirectory,

        /// <summary>
        /// Directory for basic metadata for Mp3 files.
        /// </summary>
        Mp3Directory,

        /// <summary>
        /// Directory for header metadata for Netpbm files.
        /// </summary>
        NetpbmHeaderDirectory,

        /// <summary>
        /// Directory for picture control tags specific to Nikon (type 1) cameras.
        /// </summary>
        NikonPictureControl1Directory,

        /// <summary>
        /// Directory for picture control tags specific to Nikon (type 1) cameras.
        /// </summary>
        NikonPictureControl2Directory,

        /// <summary>
        /// Directory for tags specific to Nikon (type 1) cameras.
        /// </summary>
        NikonType1MakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Nikon (type 1) cameras.
        /// </summary>
        NikonType2MakernoteDirectory,

        /// <summary>
        /// Directory for camera settings tags specific to Olympus cameras (Epson, Konica, Minolta and Agfa...).
        /// </summary>
        OlympusCameraSettingsMakernoteDirectory,

        /// <summary>
        /// Directory for equipment tags specific to Olympus cameras (Epson, Konica, Minolta and Agfa...).
        /// </summary>
        OlympusEquipmentMakernoteDirectory,

        /// <summary>
        /// Directory for focus info tags specific to Olympus cameras (Epson, Konica, Minolta and Agfa...).
        /// </summary>
        OlympusFocusInfoMakernoteDirectory,

        /// <summary>
        /// Directory for image processing tags specific to Olympus cameras (Epson, Konica, Minolta and Agfa...).
        /// </summary>
        OlympusImageProcessingMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Olympus cameras (Epson, Konica, Minolta and Agfa...).
        /// </summary>
        OlympusMakernoteDirectory,

        /// <summary>
        /// Directory for raw development 2 makernotes specific to Olympus cameras (Epson, Konica, Minolta and Agfa...).
        /// </summary>
        OlympusRawDevelopment2MakernoteDirectory,

        /// <summary>
        /// Directory for raw development 2 makernotes specific to Olympus cameras (Epson, Konica, Minolta and Agfa...).
        /// </summary>
        OlympusRawDevelopmentMakernoteDirectory,

        /// <summary>
        /// Directory for raw info makernotes tags specific to Olympus cameras (Epson, Konica, Minolta and Agfa...).
        /// </summary>
        OlympusRawInfoMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Panasonic cameras.
        /// </summary>
        PanasonicMakernoteDirectory,

        /// <summary>
        /// Directory for raw distortion tags specific to Panasonic cameras.
        /// </summary>
        PanasonicRawDistortionDirectory,

        /// <summary>
        /// Directory for raw Ifd0 tags specific to Panasonic cameras.
        /// </summary>
        PanasonicRawIfd0Directory,

        /// <summary>
        /// Directory for raw info tags specific to Panasonic cameras.
        /// </summary>
        PanasonicRawWbInfo2Directory,

        /// <summary>
        /// Directory for raw info tags specific to Panasonic cameras.
        /// </summary>
        PanasonicRawWbInfoDirectory,

        /// <summary>
        /// Directory for basic metadata for Pcx files.
        /// </summary>
        PcxDirectory,

        /// <summary>
        /// Directory for tags specific to Pentax cameras.
        /// </summary>
        PentaxMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Pentax (type 2) cameras.
        /// </summary>
        PentaxType2MakernoteDirectory,

        /// <summary>
        /// Directory for the metadata found in the APPD segment of a JPEG file saved by Photoshop.
        /// </summary>
        PhotoshopDirectory,

        /// <summary>
        /// Directory for chromaticities metadata for Png files.
        /// </summary>
        PngChromaticitiesDirectory,

        /// <summary>
        /// Directory for basic metadata for Png files.
        /// </summary>
        PngDirectory,

        /// <summary>
        /// Directory for Epson proprietary metadata.
        /// </summary>
        PrintIMDirectory,

        /// <summary>
        /// Directory for header metadata for Psd files.
        /// </summary>
        PsdHeaderDirectory,

        /// <summary>
        /// Directory for QuickTime file type metadata.
        /// </summary>
        QuickTimeFileTypeDirectory,

        /// <summary>
        /// Directory for QuickTime metadata header tags.
        /// </summary>
        QuickTimeMetadataHeaderDirectory,

        /// <summary>
        /// Directory for QuickTime movie header tags.
        /// </summary>
        QuickTimeMovieHeaderDirectory,

        /// <summary>
        /// Directory for QuickTime track header metadata.
        /// </summary>
        QuickTimeTrackHeaderDirectory,

        /// <summary>
        /// Directory for tags specific to Reconyx HyperFire 2 cameras.
        /// </summary>
        ReconyxHyperFire2MakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Reconyx HyperFire cameras.
        /// </summary>
        ReconyxHyperFireMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Reconyx UltraFire cameras.
        /// </summary>
        ReconyxUltraFireMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Ricoh cameras.
        /// </summary>
        RicohMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to certain 'newer' Samsung cameras.
        /// </summary>
        SamsungType2MakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Sanyo cameras.
        /// </summary>
        SanyoMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Sigma cameras.
        /// </summary>
        SigmaMakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Sony (type 1) cameras.
        /// </summary>
        SonyType1MakernoteDirectory,

        /// <summary>
        /// Directory for tags specific to Sony (type 6) cameras.
        /// </summary>
        SonyType6MakernoteDirectory,

        /// <summary>
        /// Directory for developer metadata for Tga files.
        /// </summary>
        TgaDeveloperDirectory,

        /// <summary>
        /// Directory for extention metadata for Tga files.
        /// </summary>
        TgaExtensionDirectory,

        /// <summary>
        /// Directory for header metadata for Tga files.
        /// </summary>
        TgaHeaderDirectory,

        /// <summary>
        /// Directory for fact metadata for Wav files.
        /// </summary>
        WavFactDirectory,

        /// <summary>
        /// Directory for format metadata for Wav files.
        /// </summary>
        WavFormatDirectory,

        /// <summary>
        /// Directory for basic metadata for WebP files.
        /// </summary>
        WebPDirectory,

        /// <summary>
        /// Directory for basic metadata for Xmp files.
        /// </summary>
        XmpDirectory,

        /// <summary>
        /// Undefined directory.
        /// </summary>
        Undefined
    }
}
