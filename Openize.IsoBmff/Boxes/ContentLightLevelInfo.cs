/*
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
    /// Contains light level information about the image.
    /// </summary>
    internal class ContentLightLevelInfo : Box
    {
        /// <summary>
        /// Indicates the max picture ligth level.
        /// </summary>
        public ushort max_content_light_level;
        
        /// <summary>
        /// Indicates the picture average ligth level.
        /// </summary>
        public ushort max_pic_average_light_level;

        /// <summary>
        /// Text summary of the box.
        /// </summary>
        public new string ToString => $"{type} Max: {max_content_light_level} Avg: {max_pic_average_light_level}";

        /// <summary>
        /// Create the box object from the bitstream and box size.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <param name="size">Box size in bytes.</param>
        public ContentLightLevelInfo(BitStreamReader stream, ulong size) : base(BoxType.clli, size)
        {
            max_content_light_level = (ushort)stream.Read(16);
            max_pic_average_light_level = (ushort)stream.Read(16);
        }
    }
}
