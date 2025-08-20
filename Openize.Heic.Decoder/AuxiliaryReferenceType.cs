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
    /// Type of auxiliary reference layer.
    /// </summary>
    public enum AuxiliaryReferenceType
    {
        /// <summary>
        /// Transparency layer.
        /// Defined as "urn:mpeg:hevc:2015:auxid:1".
        /// </summary>
        Alpha,

        /// <summary>
        /// Depth map layer.
        /// Defined as "urn:mpeg:hevc:2015:auxid:2".
        /// </summary>
        DepthMap,

        /// <summary>
        /// High dynamic range layer.
        /// Defined as "urn:com:apple:photo:2020:aux:hdrgainmap".
        /// </summary>
        HdrGainMap,

        /// <summary>
        /// Layer that represents the portrait effects matte of the image.
        /// Defined as "urn:com:apple:photo:2018:aux:portraiteffectsmatte".
        /// </summary>
        PortraitEffectsMatte,

        /// <summary>
        /// Layer that represents the semantic segmentation hair matte of the image.
        /// Defined as "urn:com:apple:photo:2019:aux:semantichairmatte".
        /// </summary>
        SemanticHairMatte,

        /// <summary>
        /// Layer that represents the semantic segmentation skin matte of the image.
        /// Defined as "urn:com:apple:photo:2019:aux:semanticskinmatte".
        /// </summary>
        SemanticSkinMatte,

        /// <summary>
        /// Layer that represents the semantic segmentation teeth matte of the image.
        /// Defined as "urn:com:apple:photo:2019:aux:semanticteethmatte".
        /// </summary>
        SemanticTeethMatte,

        /// <summary>
        /// Layer that represents the semantic segmentation glasses matte of the image.
        /// Defined as "urn:com:apple:photo:2020:aux:semanticglassesmatte".
        /// </summary>
        SemanticGlassesMatte,

        /// <summary>
        /// Layer that represents the semantic segmentation sky matte of the image.
        /// Defined as "urn:com:apple:photo:2020:aux:semanticskymatte".
        /// </summary>
        SemanticSkyMatte,

        /// <summary>
        /// Defined as "tag:apple.com,2023:photo:aux:linearthumbnail".
        /// </summary>
        LinearThumbnail,

        /// <summary>
        /// Defined as "tag:apple.com,2023:photo:aux:styledeltamap".
        /// </summary>
        StyleDeltaMap,

        /// <summary>
        /// Undefined layer.
        /// </summary>
        Undefined
    }
}
