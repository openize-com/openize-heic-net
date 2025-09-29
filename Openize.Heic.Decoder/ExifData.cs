/*
 * Openize.HEIC 
 * Copyright (c) 2024-2025 Openize Pty Ltd. 
 *
 * This file is part of Openize.HEIC.
 *
 * Openize.HEIC is available under Openize license, which is
 * available along with Openize.HEIC sources.
 */

using MetadataExtractor;
using MetadataExtractor.Formats.Adobe;
using MetadataExtractor.Formats.Avi;
using MetadataExtractor.Formats.Bmp;
using MetadataExtractor.Formats.Eps;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Exif.Makernotes;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.FileType;
using MetadataExtractor.Formats.Flir;
using MetadataExtractor.Formats.GeoTiff;
using MetadataExtractor.Formats.Gif;
using MetadataExtractor.Formats.Heif;
using MetadataExtractor.Formats.Icc;
using MetadataExtractor.Formats.Ico;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Jfif;
using MetadataExtractor.Formats.Jfxx;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Mpeg;
using MetadataExtractor.Formats.Netpbm;
using MetadataExtractor.Formats.Pcx;
using MetadataExtractor.Formats.Photoshop;
using MetadataExtractor.Formats.Png;
using MetadataExtractor.Formats.QuickTime;
using MetadataExtractor.Formats.Tga;
using MetadataExtractor.Formats.Wav;
using MetadataExtractor.Formats.WebP;
using MetadataExtractor.Formats.Xmp;
using Openize.Heic.Decoder.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Openize.Heic.Decoder
{
    /// <summary>
    /// Exchangeable image file format class.
    /// Grants access to raw exif-data and to data by specific tags.
    /// </summary>
    public class ExifData
    {
        #region Public Properties

        /// <summary>
        /// List of exif directories in frame.
        /// </summary>
        public List<MetadataExtractor.Directory> DirectoriesList { get; private set; }

        /// <summary>
        /// List of all tags that present in directories.
        /// </summary>
        public List<Tag> TagList => DirectoriesList.SelectMany(d => d.Tags).ToList();

        /// <summary>
        /// Raw exif data in bytes.
        /// </summary>
        public byte[] RawBytes { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create exif-data object.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <param name="image">Parent heic image.</param>
        /// <param name="exifFrame">Exif heic frame.</param>
        /// <exception cref="DataMisalignedException"></exception>
        internal ExifData(BitStreamWithNalSupport stream, HeicImage image, HeicImageFrame exifFrame)
        {
            var locationBox = image.Header.Meta.iloc.items.First(item => item.item_ID == exifFrame.ID);

            stream.SetBytePosition(locationBox.base_offset + locationBox.extents[0].offset);
            var end = locationBox.base_offset + locationBox.extents[0].offset + locationBox.extents[0].length;

            RawBytes = new byte[locationBox.extents[0].length - 10];

            int offset = stream.Read(32); // 0x00000006
            int define = stream.Read(32); // 0x45786966 "Exif"
            int zero = stream.Read(16); // 0x0000 "\0\0"

            if (define != 0x45786966 || zero != 0x0000)
                throw new DataMisalignedException("Unexpected Exif header");

            for (int i = 0; stream.GetBitPosition() / 8 < end; i++)
            {
                RawBytes[i] = (byte)stream.Read(8);
            }

            DirectoriesList = ImageMetadataReader.ReadMetadata(new MemoryStream(RawBytes)).Where(x => !(x is FileTypeDirectory)).ToList();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the string value for the particular tag type and the directorty specified as a class.
        /// </summary>
        /// <typeparam name="T">The directory class</typeparam>
        /// <param name="tagType">The tag type identifier</param>
        /// <returns></returns>
        public string GetExifString<T>(int tagType) where T : MetadataExtractor.Directory
        {
            var directory = DirectoriesList.OfType<T>().FirstOrDefault();
            return directory?.GetDescription(tagType);
        }

        /// <summary>
        /// Returns the raw value for the particular tag type and the directorty specified as a class.
        /// </summary>
        /// <typeparam name="T">The directory class</typeparam>
        /// <param name="tagType">The tag type identifier</param>
        /// <returns></returns>
        public object GetExifRawData<T>(int tagType) where T : MetadataExtractor.Directory
        {
            var directory = DirectoriesList.OfType<T>().FirstOrDefault();
            return directory?.GetObject(tagType);
        }

        /// <summary>
        /// Returns the string value for the particular tag type and the directorty specified as a parameter.
        /// </summary>
        /// <param name="dirType">The directory type identifier</param>
        /// <param name="tagType">The tag type identifier</param>
        /// <returns></returns>
        public string GetExifString(ExifDirectoryType dirType, int tagType)
        {
            var directory = GetExifDirectory(dirType);
            return directory?.GetDescription(tagType);
        }

        /// <summary>
        /// Returns the raw value for the particular tag type and the directorty specified as a parameter.
        /// </summary>
        /// <param name="dirType">The directory type identifier</param>
        /// <param name="tagType">The tag type identifier</param>
        /// <returns></returns>
        public object GetExifRawData(ExifDirectoryType dirType, int tagType)
        {
            var directory = GetExifDirectory(dirType);
            return directory?.GetObject(tagType);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the first directory of the specified type from the exif data.
        /// </summary>
        /// <param name="dirType">The directory type identifier</param>
        /// <returns>MetadataExtractor.Directory object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thows an exception for unknown directory types.</exception>
        private MetadataExtractor.Directory GetExifDirectory(ExifDirectoryType dirType)
        {
            switch (dirType)
            {
                // ExifDirectoryBase nested:

                case ExifDirectoryType.ExifIfd0Directory:
                    return DirectoriesList.OfType<ExifIfd0Directory>().FirstOrDefault();
                case ExifDirectoryType.ExifImageDirectory:
                    return DirectoriesList.OfType<ExifImageDirectory>().FirstOrDefault();
                case ExifDirectoryType.ExifInteropDirectory:
                    return DirectoriesList.OfType<ExifInteropDirectory>().FirstOrDefault();
                case ExifDirectoryType.ExifSubIfdDirectory:
                    return DirectoriesList.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                case ExifDirectoryType.ExifThumbnailDirectory:
                    return DirectoriesList.OfType<ExifThumbnailDirectory>().FirstOrDefault();
                case ExifDirectoryType.GpsDirectory:
                    return DirectoriesList.OfType<GpsDirectory>().FirstOrDefault();

                // Directory nested:

                case ExifDirectoryType.AdobeJpegDirectory:
                    return DirectoriesList.OfType<AdobeJpegDirectory>().FirstOrDefault();
                case ExifDirectoryType.AppleMakernoteDirectory:
                    return DirectoriesList.OfType<AppleMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.AviDirectory:
                    return DirectoriesList.OfType<AviDirectory>().FirstOrDefault();
                case ExifDirectoryType.BmpHeaderDirectory:
                    return DirectoriesList.OfType<BmpHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.CanonMakernoteDirectory:
                    return DirectoriesList.OfType<CanonMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.CasioType1MakernoteDirectory:
                    return DirectoriesList.OfType<CasioType1MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.CasioType2MakernoteDirectory:
                    return DirectoriesList.OfType<CasioType2MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.DjiMakernoteDirectory:
                    return DirectoriesList.OfType<DjiMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.DuckyDirectory:
                    return DirectoriesList.OfType<DuckyDirectory>().FirstOrDefault();
                case ExifDirectoryType.EpsDirectory:
                    return DirectoriesList.OfType<EpsDirectory>().FirstOrDefault();
                case ExifDirectoryType.ErrorDirectory:
                    return DirectoriesList.OfType<ErrorDirectory>().FirstOrDefault();
                case ExifDirectoryType.FileMetadataDirectory:
                    return DirectoriesList.OfType<FileMetadataDirectory>().FirstOrDefault();
                case ExifDirectoryType.FileTypeDirectory:
                    return DirectoriesList.OfType<FileTypeDirectory>().FirstOrDefault();
                case ExifDirectoryType.FlirCameraInfoDirectory:
                    return DirectoriesList.OfType<FlirCameraInfoDirectory>().FirstOrDefault();
                case ExifDirectoryType.FlirHeaderDirectory:
                    return DirectoriesList.OfType<FlirHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.FlirMakernoteDirectory:
                    return DirectoriesList.OfType<FlirMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.FlirRawDataDirectory:
                    return DirectoriesList.OfType<FlirRawDataDirectory>().FirstOrDefault();
                case ExifDirectoryType.FujifilmMakernoteDirectory:
                    return DirectoriesList.OfType<FujifilmMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.GeoTiffDirectory:
                    return DirectoriesList.OfType<GeoTiffDirectory>().FirstOrDefault();
                case ExifDirectoryType.GifAnimationDirectory:
                    return DirectoriesList.OfType<GifAnimationDirectory>().FirstOrDefault();
                case ExifDirectoryType.GifCommentDirectory:
                    return DirectoriesList.OfType<GifCommentDirectory>().FirstOrDefault();
                case ExifDirectoryType.GifControlDirectory:
                    return DirectoriesList.OfType<GifControlDirectory>().FirstOrDefault();
                case ExifDirectoryType.GifHeaderDirectory:
                    return DirectoriesList.OfType<GifHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.GifImageDirectory:
                    return DirectoriesList.OfType<GifImageDirectory>().FirstOrDefault();
                case ExifDirectoryType.HeicImagePropertiesDirectory:
                    return DirectoriesList.OfType<HeicImagePropertiesDirectory>().FirstOrDefault();
                case ExifDirectoryType.HeicThumbnailDirectory:
                    return DirectoriesList.OfType<HeicThumbnailDirectory>().FirstOrDefault();
                case ExifDirectoryType.HuffmanTablesDirectory:
                    return DirectoriesList.OfType<HuffmanTablesDirectory>().FirstOrDefault();
                case ExifDirectoryType.IccDirectory:
                    return DirectoriesList.OfType<IccDirectory>().FirstOrDefault();
                case ExifDirectoryType.IcoDirectory:
                    return DirectoriesList.OfType<IcoDirectory>().FirstOrDefault();
                case ExifDirectoryType.IptcDirectory:
                    return DirectoriesList.OfType<IptcDirectory>().FirstOrDefault();
                case ExifDirectoryType.JfifDirectory:
                    return DirectoriesList.OfType<JfifDirectory>().FirstOrDefault();
                case ExifDirectoryType.JfxxDirectory:
                    return DirectoriesList.OfType<JfxxDirectory>().FirstOrDefault();
                case ExifDirectoryType.JpegCommentDirectory:
                    return DirectoriesList.OfType<JpegCommentDirectory>().FirstOrDefault();
                case ExifDirectoryType.JpegDirectory:
                    return DirectoriesList.OfType<JpegDirectory>().FirstOrDefault();
                case ExifDirectoryType.JpegDnlDirectory:
                    return DirectoriesList.OfType<JpegDnlDirectory>().FirstOrDefault();
                case ExifDirectoryType.KodakMakernoteDirectory:
                    return DirectoriesList.OfType<KodakMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.KyoceraMakernoteDirectory:
                    return DirectoriesList.OfType<KyoceraMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.LeicaMakernoteDirectory:
                    return DirectoriesList.OfType<LeicaMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.LeicaType5MakernoteDirectory:
                    return DirectoriesList.OfType<LeicaType5MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.Mp3Directory:
                    return DirectoriesList.OfType<Mp3Directory>().FirstOrDefault();
                case ExifDirectoryType.NetpbmHeaderDirectory:
                    return DirectoriesList.OfType<NetpbmHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.NikonPictureControl1Directory:
                    return DirectoriesList.OfType<NikonPictureControl1Directory>().FirstOrDefault();
                case ExifDirectoryType.NikonPictureControl2Directory:
                    return DirectoriesList.OfType<NikonPictureControl2Directory>().FirstOrDefault();
                case ExifDirectoryType.NikonType1MakernoteDirectory:
                    return DirectoriesList.OfType<NikonType1MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.NikonType2MakernoteDirectory:
                    return DirectoriesList.OfType<NikonType2MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.OlympusCameraSettingsMakernoteDirectory:
                    return DirectoriesList.OfType<OlympusCameraSettingsMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.OlympusEquipmentMakernoteDirectory:
                    return DirectoriesList.OfType<OlympusEquipmentMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.OlympusFocusInfoMakernoteDirectory:
                    return DirectoriesList.OfType<OlympusFocusInfoMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.OlympusImageProcessingMakernoteDirectory:
                    return DirectoriesList.OfType<OlympusImageProcessingMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.OlympusMakernoteDirectory:
                    return DirectoriesList.OfType<OlympusMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.OlympusRawDevelopment2MakernoteDirectory:
                    return DirectoriesList.OfType<OlympusRawDevelopment2MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.OlympusRawDevelopmentMakernoteDirectory:
                    return DirectoriesList.OfType<OlympusRawDevelopmentMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.OlympusRawInfoMakernoteDirectory:
                    return DirectoriesList.OfType<OlympusRawInfoMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.PanasonicMakernoteDirectory:
                    return DirectoriesList.OfType<PanasonicMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.PanasonicRawDistortionDirectory:
                    return DirectoriesList.OfType<PanasonicRawDistortionDirectory>().FirstOrDefault();
                case ExifDirectoryType.PanasonicRawIfd0Directory:
                    return DirectoriesList.OfType<PanasonicRawIfd0Directory>().FirstOrDefault();
                case ExifDirectoryType.PanasonicRawWbInfo2Directory:
                    return DirectoriesList.OfType<PanasonicRawWbInfo2Directory>().FirstOrDefault();
                case ExifDirectoryType.PanasonicRawWbInfoDirectory:
                    return DirectoriesList.OfType<PanasonicRawWbInfoDirectory>().FirstOrDefault();
                case ExifDirectoryType.PcxDirectory:
                    return DirectoriesList.OfType<PcxDirectory>().FirstOrDefault();
                case ExifDirectoryType.PentaxMakernoteDirectory:
                    return DirectoriesList.OfType<PentaxMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.PentaxType2MakernoteDirectory:
                    return DirectoriesList.OfType<PentaxType2MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.PhotoshopDirectory:
                    return DirectoriesList.OfType<PhotoshopDirectory>().FirstOrDefault();
                case ExifDirectoryType.PngChromaticitiesDirectory:
                    return DirectoriesList.OfType<PngChromaticitiesDirectory>().FirstOrDefault();
                case ExifDirectoryType.PngDirectory:
                    return DirectoriesList.OfType<PngDirectory>().FirstOrDefault();
                case ExifDirectoryType.PrintIMDirectory:
                    return DirectoriesList.OfType<PrintIMDirectory>().FirstOrDefault();
                case ExifDirectoryType.PsdHeaderDirectory:
                    return DirectoriesList.OfType<PsdHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.QuickTimeFileTypeDirectory:
                    return DirectoriesList.OfType<QuickTimeFileTypeDirectory>().FirstOrDefault();
                case ExifDirectoryType.QuickTimeMetadataHeaderDirectory:
                    return DirectoriesList.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.QuickTimeMovieHeaderDirectory:
                    return DirectoriesList.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.QuickTimeTrackHeaderDirectory:
                    return DirectoriesList.OfType<QuickTimeTrackHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.ReconyxHyperFire2MakernoteDirectory:
                    return DirectoriesList.OfType<ReconyxHyperFire2MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.ReconyxHyperFireMakernoteDirectory:
                    return DirectoriesList.OfType<ReconyxHyperFireMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.ReconyxUltraFireMakernoteDirectory:
                    return DirectoriesList.OfType<ReconyxUltraFireMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.RicohMakernoteDirectory:
                    return DirectoriesList.OfType<RicohMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.SamsungType2MakernoteDirectory:
                    return DirectoriesList.OfType<SamsungType2MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.SanyoMakernoteDirectory:
                    return DirectoriesList.OfType<SanyoMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.SigmaMakernoteDirectory:
                    return DirectoriesList.OfType<SigmaMakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.SonyType1MakernoteDirectory:
                    return DirectoriesList.OfType<SonyType1MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.SonyType6MakernoteDirectory:
                    return DirectoriesList.OfType<SonyType6MakernoteDirectory>().FirstOrDefault();
                case ExifDirectoryType.TgaDeveloperDirectory:
                    return DirectoriesList.OfType<TgaDeveloperDirectory>().FirstOrDefault();
                case ExifDirectoryType.TgaExtensionDirectory:
                    return DirectoriesList.OfType<TgaExtensionDirectory>().FirstOrDefault();
                case ExifDirectoryType.TgaHeaderDirectory:
                    return DirectoriesList.OfType<TgaHeaderDirectory>().FirstOrDefault();
                case ExifDirectoryType.WavFactDirectory:
                    return DirectoriesList.OfType<WavFactDirectory>().FirstOrDefault();
                case ExifDirectoryType.WavFormatDirectory:
                    return DirectoriesList.OfType<WavFormatDirectory>().FirstOrDefault();
                case ExifDirectoryType.WebPDirectory:
                    return DirectoriesList.OfType<WebPDirectory>().FirstOrDefault();
                case ExifDirectoryType.XmpDirectory:
                    return DirectoriesList.OfType<XmpDirectory>().FirstOrDefault();

                default:
                    throw new ArgumentOutOfRangeException("Undefined Exif directory type");
            }
        }

        #endregion
    }
}
