using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using JPEG_Cs;

namespace UnitTestsJPEG
{
    [TestClass]
    public class UnitTestsDCT
    {
        [TestMethod]
        public void ТестСдвигУровней()
        {
            byte[,] matrix = new byte[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = (byte)(i * j);
                }
            }       
            byte[,] restoredMatrix = DCT.ОбратныйСдвигУровней(DCT.СдвигУровней(matrix));
            CollectionAssert.AreEqual(matrix, restoredMatrix);
        }
        [TestMethod]
        public void ТестFDCTandIDCT()
        {
            short[,] matrix = new short[8, 8];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = (short)(i * j);
                }
            }
            short[,] IDCTMatrix = DCT.FDCT(DCT.IDCT(matrix));
            for (int i = 0; i < IDCTMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < IDCTMatrix.GetLength(1); j++)
                {
                    if ((matrix[i, j] < IDCTMatrix[i, j] - 1) || (matrix[i, j] > IDCTMatrix[i, j] + 1))
                        Assert.Fail();
                }
            }
        }
        [TestMethod]
        public void ТестЗигзага()
        {
            short[] array = new short[64];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                array[i] = (short)i;
            }
            short[] restoredArray = DCT.Зигзаг(DCT.ОбратныйЗигзаг(array));
            CollectionAssert.AreEqual(array, restoredArray);
        }
        [TestMethod]
        public void ТестКвантования()
        {
            short[,] coefficient = new short[8, 8];
            for (int i = 0; i < coefficient.GetLength(0); i++)
            {
                for (int j = 0; j < coefficient.GetLength(1); j++)
                {
                    coefficient[i, j] = (short)(i * j);
                }
            }
            short[,] quantization = new short[8, 8];
            for (int i = 0; i < quantization.GetLength(0); i++)
            {
                for (int j = 0; j < quantization.GetLength(1); j++)
                {
                    quantization[i, j] = (short)(1 + 1 + i + j);
                }
            }
            short[,] coefficientMatrix = coefficient;
            DCT.Квантование(coefficient, quantization);
            DCT.ОбратноеКвантование(coefficient, quantization);
            short[,] restoredCoefficientMatrix = coefficient;
            for (int i = 0; i < coefficient.GetLength(0); i++)
            {
                for (int j = 0; j < coefficient.GetLength(1); j++)
                {
                    if ((coefficientMatrix[i, j] < restoredCoefficientMatrix[i, j] - 4) || (coefficientMatrix[i, j] > restoredCoefficientMatrix[i, j] + 4))
                        Assert.Fail();
                }
            }
        }
    }
}
