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
    /// Specifies the format of the color data for each pixel in the image.
    /// </summary>
    public enum PixelFormat
    {
        /// <summary>
        /// Specifies that the format is 24 bits per pixel; 8 bits each are used for the red, green, and blue components.
        /// </summary>
        Rgb24,

        /// <summary>
        /// Specifies that the format is 32 bits per pixel; 8 bits each are used for the red, green, blue, and alpha components.
        /// </summary>
        Rgba32,

        /// <summary>
        /// Specifies that the format is 32 bits per pixel; 8 bits each are used for the alpha, red, green, and blue components.
        /// </summary>
        Argb32,

        /// <summary>
        /// Specifies that the format is 32 bits per pixel; 8 bits each are used for the blue, green, red, and alpha components.
        /// </summary>
        Bgra32,
    }
}
