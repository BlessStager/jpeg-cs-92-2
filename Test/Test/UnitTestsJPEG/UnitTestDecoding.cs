using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using JPEG_Cs;
using System.IO;

namespace UnitTestsJPEG
{
    [TestClass]
    public class UnitTestDecoding
    {
        [TestMethod]
        public void NextBit()
        {
            FileStream stream = File.Open("Penguins.jpg", FileMode.Open);
            Decoding decoding = new Decoding(stream, null, null);
            stream.Seek(0x3d6, SeekOrigin.Begin);
            byte expectedResult = (byte)stream.ReadByte();
            stream.Position--;
            byte result = 0;
            for (int i = 0; i < 8; i++)
            {
                byte x = decoding.NextBit();
                byte temp = (byte)(x << (7 - i));
                result += temp;
            }
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Extend()
        {
            FileStream stream = File.Open("Penguins.jpg", FileMode.Open);
            Decoding decoding = new Decoding(stream, null, null);

            ushort[] diffs = new ushort[6] { 1, 0, 2, 3, 1, 0 };
            int[] nums = new int[6] { 1, 1, 2, 2, 2, 2 };
            short[] referenceResults = new short[6] { 1, -1, 2, 3, -2, -3 };

            for (int i = 0; i < 6; i++)
            {
                ushort diff = diffs[i];
                int num_bits = nums[i];
                short referenceResult = referenceResults[i];
                short result = decoding.Extend(diff, num_bits);

                Assert.AreEqual(referenceResult, result);
            }
        }
    }
}
