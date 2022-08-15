using System;
using System.Collections.Generic;
using System.Text;

namespace JPEG_Cs
{
    /// <summary>
    /// Статический класс, содержащий статические методы, осуществляющие DCT преобразования.
    /// </summary>
    public static class DCT
    {
        /// <summary>
        /// Метод, который переводит каждый компонент матрицы из диапазона 0..255 в диапазон -128..127
        /// </summary>
        /// <returns>
        /// Возвращает преобразованную матрицу.
        /// </returns>
        public static short[,] СдвигУровней(byte[,] матрица)
        {
            short[,] обМатрица = new short[матрица.GetLength(0), матрица.GetLength(1)];
            for (int i = 0; i < матрица.GetLength(0); i++)
            {
                for (int j = 0; j < матрица.GetLength(1); j++)
                {
                    обМатрица[i, j] = (short)(матрица[i, j] - 128);
                }
            }
            return обМатрица;
        }

        /// <summary>
        /// Метод, который переводит каждый компонент матрицы из диапазона -128..127 в диапазон 0..255 
        /// </summary>
        /// <returns>
        /// Возвращает преобразованную матрицу.
        /// </returns>
        public static byte[,] ОбратныйСдвигУровней(short[,] обМатрица)
        {
            byte[,] матрица = new byte[обМатрица.GetLength(0), обМатрица.GetLength(1)];
            for (int i = 0; i < обМатрица.GetLength(0); i++)
            {
                for (int j = 0; j < обМатрица.GetLength(1); j++)
                {
                    матрица[i, j] = (byte)(обМатрица[i, j] + 128);
                }
            }
            return матрица;
        }

        /// <summary>
        /// Метод, который производит квантование
        /// </summary>
        /// <param name="матрицаКоэффициентов">матрицаКоэффициентов</param>
        /// <param name="матрицаКвантования">матрицаКвантования</param>
        /// <param name="x">Ширина</param>
        /// <param name="y">Высота</param>
        public static void Квантование(short[,] матрицаКоэффициентов, short[,] матрицаКвантования)
        {
            const int ШИРИНА = 8;
            const int ВЫСОТА = 8;
            for (int i = 0; i < ШИРИНА; i++)
            {
                for (int j = 0; j < ВЫСОТА; j++)
                {
                    double left = матрицаКоэффициентов[i, j];
                    double right = матрицаКвантования[i, j];
                    double res = Convert.ToDouble(матрицаКвантования[i, j]) / 2;
                    if (left % right < res)
                    {
                        матрицаКоэффициентов[i, j] /= матрицаКвантования[i, j];
                    }
                    else
                    {
                        матрицаКоэффициентов[i, j] /= матрицаКвантования[i, j];
                        матрицаКоэффициентов[i, j]++;
                    }
                }
            }
        }

        /// <summary>
        /// Метод, который производит обратное квантование
        /// </summary>
        /// <param name="матрицаКоэффициентов">матрицаКоэффициентов</param>
        /// <param name="матрицаКвантования">матрицаКвантования</param>
        /// <param name="x">Ширина и высота</param>
        public static void ОбратноеКвантование(short[,] матрицаКоэффициентов, short[,] матрицаКвантования)
        {
            const int ШИРИНА = 8;
            const int ВЫСОТА = 8;
            for (int i = 0; i < ШИРИНА; i++)
            {
                for (int j = 0; j < ВЫСОТА; j++)
                {
                    матрицаКоэффициентов[i, j] *= матрицаКвантования[i, j];
                }
            }
        }

        /// <summary>
        /// Метод, который производит прямое DCT преобразование матрицы.
        /// </summary>
        /// <returns>
        /// Возвращает преобразованную матрицу.
        /// </returns>
        public static short[,] FDCT(short[,] matrix)
        {
            short[,] dctMatrix = new short[matrix.GetLength(0), matrix.GetLength(1)];
            int N = matrix.GetLength(0);
            int P = matrix.GetLength(1);
            double cu;
            double cv;
            for (int u = 0; u < N; u++)
            {
                for (int v = 0; v < P; v++)
                {
                    if (u == 0)
                    {
                        cu = 1 / Math.Sqrt(2);
                    }
                    else
                    {
                        cu = 1;
                    }
                    if (v == 0)
                    {
                        cv = 1 / Math.Sqrt(2);
                    }
                    else
                    {
                        cv = 1;
                    }
                    double cos = 0;
                    for (int x = 0; x < N; x++)
                    {
                        for (int y = 0; y < P; y++)
                        {
                            cos += matrix[y, x] * Math.Cos((2 * x + 1) * u * Math.PI / 16.0) * Math.Cos((2 * y + 1) * v * Math.PI / 16.0);
                        }
                    }
                    dctMatrix[v, u] = Convert.ToInt16(cu * cv  * cos / 4.0);
                }
            }
            return dctMatrix;
        }

        /// <summary>
        /// Метод, который производит обратное DCT преобразование матрицы.
        /// </summary>
        /// <returns>
        /// Возвращает преобразованную матрицу.
        /// </returns>
        public static short[,] IDCT(short[,] dctMatrix)
        {
            short[,] idctMatrix = new short[dctMatrix.GetLength(0), dctMatrix.GetLength(1)];
            int N = dctMatrix.GetLength(0);
            int P = dctMatrix.GetLength(1);
            double cu;
            double cv;
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < P; y++)
                {
                    double cos = 0;
                    for (int u = 0; u < N; u++)
                    {
                        for (int v = 0; v < P; v++)
                        {
                            if (u == 0)
                            {
                                cu = 1 / Math.Sqrt(2);
                            }
                            else
                            {
                                cu = 1;
                            }
                            if (v == 0)
                            {
                                cv = 1 / Math.Sqrt(2);
                            }
                            else
                            {
                                cv = 1;
                            }
                            cos += cu * cv * dctMatrix[v, u] * Math.Cos((2 * x + 1) * u * Math.PI / 16.0) * Math.Cos((2 * y + 1) * v * Math.PI / 16.0);
                        }
                    }
                    idctMatrix[y, x] = Convert.ToInt16(cos / 4.0);
                }
            }
            return idctMatrix;
        }

        /// <summary>
        /// Метод, который реализует зигзагообразный обход матрицы и записывает получившуюся последовательность в массив.
        /// </summary>
        /// <returns>
        /// Возвращает массив.
        /// </returns>
        public static short[] Зигзаг(short[,] matrix)
        {
            List<short> reMatrix = new List<short>();
            List<short[,]> listMatrix = new List<short[,]>();
            for (int i = 0; i < 8; i++)
            {
                listMatrix.Add(ВспомогательноеРазбиение(matrix, i + 1));
            }
            for (int i = 6; i >= 0; i--)
            {
                listMatrix.Add(ОбратноеВспомогательноеРазбиение(matrix, i + 1));
            }
            foreach (var m in listMatrix)
            {
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    if (m.GetLength(0) % 2 == 0)
                    {
                        reMatrix.Add(m[i, m.GetLength(0) - i - 1]);
                    }
                    else
                    {
                        reMatrix.Add(m[m.GetLength(0) - i - 1, i]);
                    }
                }
            }
            short[] reMatrixArray = reMatrix.ToArray();
            return reMatrixArray;
        }

        /// <summary>
        /// Метод, который восстанавливает матрицу, используя массив, полученный её зигзагообразным обходом.
        /// </summary>
        /// <returns>
        /// Возвращает восстановленную матрицу.
        /// </returns>
        public static short[,] ОбратныйЗигзаг(short[] vec)
        {          
            short[,] reVec = new short[8, 8];
            int beg = 0;
            for (int i = 0; i < 8; i++)
            {               
                short[,] hMatr = ВспомогательноеСлияние(vec, i + 1, beg);
                for (int j = 0; j < hMatr.GetLength(0); j++)
                {
                    reVec[j, hMatr.GetLength(0) - j - 1] = hMatr[j, hMatr.GetLength(0) - j - 1];
                }
                beg += i + 1;
            }
            for (int i = 6; i >= 0; i--)
            {
                short[,] hMatr = ВспомогательноеСлияние(vec, i + 1, beg);
                for (int j = 0; j < hMatr.GetLength(0); j++)
                {
                    reVec[j + 7 - i, hMatr.GetLength(0) - j - 1 + 7 - i] = hMatr[j, hMatr.GetLength(0) - j - 1];
                }
                beg += i + 1;
            }
            return reVec;
        }

        private static short[,] ВспомогательноеРазбиение(short[,] matrix, int dim)
        {
            short[,] reMatrix = new short[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    reMatrix[j, i] = matrix[i, j];
                }
            }
            return reMatrix;
        }

        private static short[,] ОбратноеВспомогательноеРазбиение(short[,] matrix, int dim)
        {
            short[,] reMatrix = new short[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    reMatrix[i, j] = matrix[7 - dim + 1 + j, 7 - dim + 1 + i];
                }
            }
            return reMatrix;
        }

        private static short[,] ВспомогательноеСлияние(short[] vec, int dim, int beg)
        {
            short[,] reMatrix = new short[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                if (dim % 2 == 0)
                {
                    reMatrix[dim - i - 1, i] = vec[beg + i];
                }
                else
                {
                    reMatrix[i, dim - i - 1] = vec[beg + i];
                }
            }
            return reMatrix;
        }
    }
}