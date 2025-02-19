﻿/*
 * Openize.IsoBmff
 * Copyright (c) 2024-2025 Openize Pty Ltd. 
 *
 * This file is part of Openize.IsoBmff.
 *
 * Openize.IsoBmff is available under MIT license, which is
 * available along with Openize.IsoBmff sources.
 */

using Openize.IsoBmff.IO;

namespace Openize.IsoBmff
{
    /// <summary>
    /// The image rotation  transformative item property rotates the reconstructed image of the
    /// associated image item in anti-clockwise direction in units of 90 degrees.
    /// </summary>
    public class ImageRotation : Box
    {
        /// <summary>
        /// Specifies the angle (in anti-clockwise direction) in units of 90 degrees.
        /// </summary>
        public byte angle;

        /// <summary>
        /// Text summary of the box.
        /// </summary>
        public new string ToString => $"{type} angle: {angle * 90}";

        /// <summary>
        /// Create the box object from the bitstream and box size.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <param name="size">Box size in bytes.</param>
        public ImageRotation(BitStreamReader stream, ulong size) : base(BoxType.irot, size)
        {
            stream.SkipBits(6);
            angle = (byte)stream.Read(2);
        }
    }
}
