/*
 * Openize.HEIC 
 * Copyright (c) 2024-2025 Openize Pty Ltd. 
 *
 * This file is part of Openize.HEIC.
 *
 * Openize.HEIC is available under Openize license, which is
 * available along with Openize.HEIC sources.
 */

using MetadataExtractor.Formats.Exif;
using Openize.Heic.Decoder.IO;
using Openize.IsoBmff;
using Openize.IsoBmff.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Openize.Heic.Decoder
{
    /// <summary>
    /// Heic image class.
    /// </summary>
    public class HeicImage
    {
        /// <summary>
        /// Image stream with NAL unit specification reader.
        /// </summary>
        private BitStreamWithNalSupport _stream;

        /// <summary>
        /// Dictionary of Heic image frames.
        /// </summary>
        private Dictionary<uint, HeicImageFrame> _frames;

        #region Constructors

        /// <summary>
        /// Create heic image object.
        /// </summary>
        /// <param name="header">File header.</param>
        /// <param name="stream">File stream.</param>
        private HeicImage(HeicHeader header, BitStreamWithNalSupport stream)
        {
            this.Header = header;
            this._stream = stream;

            _frames = new Dictionary<uint, HeicImageFrame>();

            ReadFramesMeta(stream);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Heic image header. Grants convinient access to IsoBmff container meta data.
        /// </summary>
        public HeicHeader Header { get; private set; }

        /// <summary>
        /// Dictionary of public Heic image frames with access by identifier.
        /// </summary>
        public Dictionary<uint, HeicImageFrame> Frames
        {
            get
            {
                var frames = _frames
                    .Where(f => 
                        (!f.Value.IsHidden) && 
                        (f.Value.DerivativeType != BoxType.thmb) &&
                        (f.Value.ImageType != ImageFrameType.tmap))
                    .ToDictionary(mc => mc.Key, mc => mc.Value);

                var groups = this.Header.GetGroupsIfPresent();
                if (groups == null)
                    return frames;

                foreach (var grp in groups)
                {
                    if (grp.type == BoxType.altr)
                    {
                        var entities = grp.entities;
                        bool selected = false;
                        
                        for (int i = 0; i < entities.Length; i++)
                        {
                            if (frames.ContainsKey(entities[i]))
                            {
                                if (!selected)
                                    selected = true;
                                else
                                    frames.Remove(entities[i]);
                            }
                        }
                    }
                }

                return frames;
            }
        }

        /// <summary>
        /// Dictionary of all Heic image frames with access by identifier.
        /// </summary>
        public Dictionary<uint, HeicImageFrame> AllFrames => _frames;

        /// <summary>
        /// Returns the default image frame, which is specified in meta data.
        /// </summary>
        public HeicImageFrame DefaultFrame => _frames[Header.DefaultFrameId];

        /// <summary>
        /// Width of the default image frame in pixels.
        /// </summary>
        public uint Width => DefaultFrame.Width;

        /// <summary>
        /// Height of the default image frame in pixels.
        /// </summary>
        public uint Height => DefaultFrame.Height;

        /// <summary>
        /// Exchangeable image metadata of the default image frame. 
        /// </summary>
        public ExifData Exif => DefaultFrame.Exif;

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the file meta data and creates a class object for further decoding of the file contents.
        /// <para>This operation does not decode pixels.
        /// Use the default frame methods GetByteArray or GetInt32Array afterwards in order to decode pixels.</para>
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <returns>Returns a heic image object with meta data read.</returns>
        public static HeicImage Load(Stream stream)
        {
            var bitstream = new BitStreamWithNalSupport(stream, 4);
            bitstream.SetBytePosition(0);

            Box.SetExternalConstructor(BoxType.hvcC, (BitStreamReader str, ulong size) => {
                if (!(str is BitStreamWithNalSupport nalStream))
                    throw new Exception("Stream dedication logic error.");
                return new HEVCConfigurationBox(nalStream, size);
            });

            while (bitstream.MoreData())
            {
                var box = Box.ParseBox(bitstream);

                if (box.type == BoxType.meta)
                    return new HeicImage(new HeicHeader(box as MetaBox), bitstream);
            }

            throw new Exception("Meta box not found.");
        }

        /// <summary>
        /// Checks if the stream can be read as a heic image.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <returns>True if file header contains heic signarure, false otherwise.</returns>
        public static bool CanLoad(Stream stream)
        {
            var bitstream = new BitStreamWithNalSupport(stream);

            var box = Box.ParseBox(bitstream);

            if (!(box is FileTypeBox filetype))
                return false;

            if (!filetype.IsBrandSupported(1751476579)) // heic (ASCII)
                return false;

            bitstream.SetBytePosition(0);

            return true;
        }

        /// <summary>
        /// Get pixel data of the default image frame in the format of byte array.
        /// <para>Each three or four bytes (the count depends on the pixel format) refer to one pixel left to right top to bottom line by line.</para>
        /// </summary>
        /// <param name="pixelFormat">Pixel format that defines the order of colors and the presence of alpha byte.</param>
        /// <param name="boundsRectangle">Bounds of the requested area.</param>
        /// <returns>Byte array, null if frame does not contain image data.</returns>
        public byte[] GetByteArray(PixelFormat pixelFormat, Rectangle boundsRectangle = default) => DefaultFrame.GetByteArray(pixelFormat, boundsRectangle);

        /// <summary>
        /// Get pixel data of the default image frame in the format of integer array. 
        /// <para>Each int value refers to one pixel left to right top to bottom line by line.</para>
        /// </summary>
        /// <param name="pixelFormat">Pixel format that defines the order of colors.</param>
        /// <param name="boundsRectangle">Bounds of the requested area.</param>
        /// <returns>Integer array, null if frame does not contain image data.</returns>
        public int[] GetInt32Array(PixelFormat pixelFormat, Rectangle boundsRectangle = default) => DefaultFrame.GetInt32Array(pixelFormat, boundsRectangle);

        #endregion

        #region Private Methods

        /// <summary>
        /// Fill frames dictionary with read meta data.
        /// </summary>
        /// <param name="stream">File stream.</param>
        private void ReadFramesMeta(BitStreamWithNalSupport stream)
        {
            var rawProperties = Header.GetProperties();

            foreach (var item in Header.Meta.iloc.items)
            {
                uint id = item.item_ID;
                _frames.Add(id, new HeicImageFrame(stream, this, id));
            }

            foreach (var frame in _frames)
            {
                if (rawProperties.ContainsKey(frame.Value.ID))
                    frame.Value.LoadProperties(stream, rawProperties[frame.Value.ID]);

                if (frame.Value.ImageType == ImageFrameType.Exif)
                {
                    var derived = Header.GetDerivedList(frame.Value.ID);
                    var exif = new ExifData(stream, this, frame.Value);

                    foreach (var derivedId in derived)
                    {
                        _frames[derivedId].Exif = exif;
                    }
                }
            }
        }

        #endregion
    }
}
