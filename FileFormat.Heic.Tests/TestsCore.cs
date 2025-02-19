/*
 * FileFormat.HEIC 
 * Copyright (c) 2024 Openize Pty Ltd. 
 *
 * This file is part of FileFormat.HEIC.
 *
 * FileFormat.HEIC is available under Openize license, which is
 * available along with FileFormat.HEIC sources.
 */

namespace FileFormat.Heic.Tests
{
    using NUnit.Framework;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Intrinsics;

    public class TestsCore
    {
        /// <summary>
        /// Samples path.
        /// </summary>
        protected string SamplesPath { get; set; }

        /// <summary>
        /// Ethalons path.
        /// </summary>
        protected string EthalonsPath { get; set; }

        /// <summary>
        /// Startup setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            SamplesPath = GetSamplesPath();
            EthalonsPath = GetEthalonsPath();
        }

        /// <summary>
        /// Create ethalon file.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <param name="data">Color data.</param>
        protected void CreateEthalon(string filename, byte[] data)
        {
            var outputFilename = filename + ".bin";

            using (var stream = new FileStream(Path.Combine(EthalonsPath, outputFilename), FileMode.OpenOrCreate, FileAccess.Write))
            {
                stream.Write(data, 0, data.Length);
            }
        }

        /// <summary>
        /// Compare color data with ethalon file.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <param name="data">Color data.</param>
        protected void CompareWithEthalon(string filename, byte[] data)
        {
            var outputFilename = filename + ".bin";

            using (var stream = new FileStream(Path.Combine(EthalonsPath, outputFilename), FileMode.Open))
            {
                const int bytesToRead = 32;
                int index = 0;

                if (stream.Length != data.Length)
                {
                    Assert.Fail($"Ethalon length do not match. Ethalon length equals {stream.Length}, read data length equals {data.Length}");
                }

                var one = new byte[bytesToRead];
                var two = new byte[bytesToRead];

                var canRead = true;
                while (canRead)
                {
                    var read_bytes_from_stream = stream.Read(one, 0, bytesToRead);

                    if (index + bytesToRead <= data.Length)
                        Array.Copy(data, index, two, 0, bytesToRead);
                    else
                        Array.Copy(data, index, two, 0, data.Length - index);

                    index += bytesToRead;

                    var vOne = MemoryMarshal.Cast<byte, Vector256<byte>>(one);
                    var vTwo = MemoryMarshal.Cast<byte, Vector256<byte>>(two);

                    if (!vTwo.SequenceEqual(vOne))
                    {
                        Assert.Fail("Data does not match ethalon");
                        return;
                    }

                    canRead = read_bytes_from_stream == bytesToRead && index < data.Length;
                }

                Assert.Pass();
            }
        }

        /// <summary>
        /// Get project path.
        /// </summary>
        private static string GetProjectPath()
        {
            var path = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            var binIndex = Math.Max(path.IndexOf("/bin"), path.IndexOf("\\bin"));
            return path.Remove(binIndex);
        }

        /// <summary>
        /// Get test samples path.
        /// </summary>
        private static string GetSamplesPath()
        {
            return Path.Combine(GetProjectPath(), "TestsData", "samples");
        }

        /// <summary>
        /// Get test ethalons path.
        /// </summary>
        private static string GetEthalonsPath()
        {
            return Path.Combine(GetProjectPath(), "TestsData", "ethalons");
        }

    }
}