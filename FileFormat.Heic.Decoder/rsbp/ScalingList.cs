/*
 * FileFormat.HEIC 
 * Copyright (c) 2024 Openize Pty Ltd. 
 *
 * This file is part of FileFormat.HEIC.
 *
 * FileFormat.HEIC is available under Openize license, which is
 * available along with FileFormat.HEIC sources.
 */

namespace FileFormat.Heic.Decoder
{
    internal class Scaling
    {
        // Table 7-3 – Specification of sizeId
        //
        // sizeId   Size of quantization matrix
        // 0        4x4
        // 1        8x8
        // 2        16x16
        // 3        32x32

        // Table 7-4 – Specification of matrixId according to sizeId, prediction mode and colour component 
        //
        // sizeId       CuPredMode   cIdx   matrixId
        // 0, 1, 2, 3   MODE_INTRA  0 (Y)   0
        // 0, 1, 2, 3   MODE_INTRA  1 (Cb)  1
        // 0, 1, 2, 3   MODE_INTRA  2 (Cr)  2
        // 0, 1, 2, 3   MODE_INTER  0 (Y)   3
        // 0, 1, 2, 3   MODE_INTER  1 (Cb)  4
        // 0, 1, 2, 3   MODE_INTER  2 (Cr)  5

        internal static int[][][,] ScalingFactor { get; private set; }

        static readonly int[] scaling_list_4x4 = {
            16,16,16,16,
            16,16,16,16,
            16,16,16,16,
            16,16,16,16
        };

        static readonly int[] scaling_list_8x8_intra = {
            16,16,16,16,16,16,16,16,
            16,16,17,16,17,16,17,18,
            17,18,18,17,18,21,19,20,
            21,20,19,21,24,22,22,24,
            24,22,22,24,25,25,27,30,
            27,25,25,29,31,35,35,31,
            29,36,41,44,41,36,47,54,
            54,47,65,70,65,88,88,115
        };

        static readonly int[] scaling_list_8x8_inter = {
            16,16,16,16,16,16,16,16,
            16,16,17,17,17,17,17,18,
            18,18,18,18,18,20,20,20,
            20,20,20,20,24,24,24,24,
            24,24,24,24,25,25,25,25,
            25,25,25,28,28,28,28,28,
            28,33,33,33,33,33,41,41,
            41,41,54,54,54,71,71,91
        };

        internal static void Initiate(seq_parameter_set_rbsp sps)
        {
            if (ScalingFactor != null) return;

            ScalingFactor = new int[4][][,];

            for (int i = 0; i < 4; i++)
                ScalingFactor[i] = new int[6][,];

            int sizeId = 0, x = 0, y = 0;

            for (int matrixId = 0; matrixId < 6; matrixId++)
            {
                ScalingFactor[0][matrixId] = new int[1 << (2 + sizeId), 1 << (2 + sizeId)];

                for (int i = 0; i < 16; i++)
                {
                    x = Scans.ScanOrder[2][0][i][0];
                    y = Scans.ScanOrder[2][0][i][1];
                    ScalingFactor[0][matrixId][x, y] = scaling_list_4x4[i]; 
                }
            }

            sizeId = 1;
            for (int matrixId = 0; matrixId < 6; matrixId++)
            {
                ScalingFactor[1][matrixId] = new int[1 << (2 + sizeId), 1 << (2 + sizeId)];

                for (int i = 0; i < 64; i++)
                {
                    x = Scans.ScanOrder[3][0][i][0];
                    y = Scans.ScanOrder[3][0][i][1];

                    if (matrixId < 3)
                        ScalingFactor[1][matrixId][x, y] = scaling_list_8x8_intra[i];
                    else
                        ScalingFactor[1][matrixId][x, y] = scaling_list_8x8_inter[i];
                }
            }

            sizeId = 2;
            for (int matrixId = 0; matrixId < 6; matrixId++)
            {
                ScalingFactor[2][matrixId] = new int[1 << (2 + sizeId), 1 << (2 + sizeId)];

                for (int i = 0; i < 64; i++)
                {
                    x = Scans.ScanOrder[3][0][i][0];
                    y = Scans.ScanOrder[3][0][i][1];

                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            if (matrixId < 3)
                                ScalingFactor[2][matrixId][x * 2 + k, y * 2 + j] = scaling_list_8x8_intra[i];
                            else
                                ScalingFactor[2][matrixId][x * 2 + k, y * 2 + j] = scaling_list_8x8_inter[i];
                        }
                    }
                }

                if (sps.sps_scaling_list_data_present_flag)
                    ScalingFactor[2][matrixId][0, 0] = sps.scaling_list_data.scaling_list_dc_coef_minus8[0][matrixId] + 8;
            }

            sizeId = 3;
            for (int matrixId = 0; matrixId < 6; matrixId++)
            {
                ScalingFactor[3][matrixId] = new int[1 << (2 + sizeId), 1 << (2 + sizeId)];

                for (int i = 0; i < 64; i++)
                {
                    x = Scans.ScanOrder[3][0][i][0];
                    y = Scans.ScanOrder[3][0][i][1];

                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if (matrixId < 3)
                                ScalingFactor[3][matrixId][x * 4 + k, y * 4 + j] = scaling_list_8x8_intra[i];
                            else
                                ScalingFactor[3][matrixId][x * 4 + k, y * 4 + j] = scaling_list_8x8_inter[i];
                        }
                    }
                }

                if (sps.sps_scaling_list_data_present_flag)
                {
                    if (matrixId == 0 || matrixId == 3)
                        ScalingFactor[3][matrixId][0, 0] = sps.scaling_list_data.scaling_list_dc_coef_minus8[1][matrixId] + 8;
                    else
                        ScalingFactor[3][matrixId][0, 0] = sps.scaling_list_data.scaling_list_dc_coef_minus8[0][matrixId] + 8;
                }
            }
        }
    }
}
