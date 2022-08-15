using JPEG_Cs;
using System;
using System.Collections.Generic;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //ПроверкаЗигзага();
            //Тест_RGBtoYUV();
            //ТестJPEGFile();
            ТестКанал1();
            //ТестHuffmanTable();
        }

        private static void Тест_RGBtoYUV()
        {
            const int ширина = 10;
            const int высота = 10;
            JPEG_Cs.JPEG_Cs jpeg = CreateJpeg();

            Точка[,] изображение = new Точка[ширина, высота];
            изображение = ЗаполнитьИзображениеRGB(изображение);
            PrintRGB(изображение);

            byte[,] Y;
            byte[,] Cb;
            byte[,] Cr;
            jpeg.RGBtoYUV(изображение, out Y, out Cb, out Cr);
            //PrintYUV(Y, Cb, Cr);

            Точка[,] изображение2 = jpeg.YUVtoRGB(Y, Cb, Cr);
            PrintRGB(изображение2);
        }

        private static JPEG_Cs.JPEG_Cs CreateJpeg()
        {
            return new JPEG_Cs.JPEG_Cs(File.Open("example.txt", FileMode.Create));
        }

        private static Точка[,] ЗаполнитьИзображениеRGB(Точка[,] изображение)
        {
            var random = new Random();
            int ширинаИзображения = изображение.GetLength(0);
            int высотаИзображения = изображение.GetLength(1);

            for (int j = 0; j < высотаИзображения; j++)
                for (int i = 0; i < ширинаИзображения; i++)
                {
                    изображение[i, j].r = (byte)random.Next(0, 256);
                    изображение[i, j].g = (byte)random.Next(0, 256);
                    изображение[i, j].b = (byte)random.Next(0, 256);
                }

            return изображение;
        }

        private static void PrintRGB(Точка[,] изображение)
        {
            int ширинаИзображения = изображение.GetLength(0);
            int высотаИзображения = изображение.GetLength(1);

            Console.WriteLine("Компонента R:");
            for (int j = 0; j < высотаИзображения; j++)
            {
                for (int i = 0; i < ширинаИзображения; i++)
                {
                    byte r = изображение[i, j].r;
                    if (r < 10) Console.Write($"00{r}");
                    else if (r < 100) Console.Write($"0{r}");
                    else Console.Write(r);

                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Компонента G:");
            for (int j = 0; j < высотаИзображения; j++)
            {
                for (int i = 0; i < ширинаИзображения; i++)
                {
                    byte g = изображение[i, j].g;
                    if (g < 10) Console.Write($"00{g}");
                    else if (g < 100) Console.Write($"0{g}");
                    else Console.Write(g);

                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Компонента B:");
            for (int j = 0; j < высотаИзображения; j++)
            {
                for (int i = 0; i < ширинаИзображения; i++)
                {
                    byte b = изображение[i, j].b;
                    if (b < 10) Console.Write($"00{b}");
                    else if (b < 100) Console.Write($"0{b}");
                    else Console.Write(b);

                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void PrintYUV(byte[,] Y, byte[,] Cb, byte[,] Cr)
        {
            Console.WriteLine("Компонента Y:");
            ВывестиМатрицуБайтовНаКонсоль(Y);

            Console.WriteLine("Компонента Cb:");
            ВывестиМатрицуБайтовНаКонсоль(Cb);

            Console.WriteLine("Компонента Cr:");
            ВывестиМатрицуБайтовНаКонсоль(Cr);
        }

        static byte[,] матрица = new byte[13, 13];
        static Канал канал = new Канал(матрица, 2, 2);

        private static void Тест_FDCT_и_IDCT()
        {
            JPEG_Cs.JPEG_Cs jpg = new JPEG_Cs.JPEG_Cs(File.Open("hello.txt", FileMode.Create));
            const int H = 2;
            const int V = 2;
            const int X = 27;
            const int Y = 3;

            byte[,] матрица = new byte[X, Y];
            ЗаполнитьМатрицуСлучайнымиЗначениями(матрица);
            Канал канал1 = new Канал(матрица, H, V); 
            Console.WriteLine("Исходная матрица:");
            ВывестиМатрицуБайтовНаКонсоль(канал1.ПолучитьМатрицу());

            short[,] матрицаКвантования = new short[8, 8];
            ЗаполнитьМатрицуКвантования(матрицаКвантования);

            var списокБлоков = канал1.РазбитьМатрицуНаБлокиВФормеСписка(8, 8);
            var списокКоэффициентов = jpg.FDCT(списокБлоков, матрицаКвантования);

            var восстановленныйСписокБлоков = jpg.IDCT(списокКоэффициентов, матрицаКвантования);
            Канал канал2 = new Канал(восстановленныйСписокБлоков, матрица.GetLength(0), матрица.GetLength(1), H, V);
            Console.WriteLine("Восстановленная матрица:");
            ВывестиМатрицуБайтовНаКонсоль(канал2.ПолучитьМатрицу());
        }

        private static void ТестМасштабирование()
        {
            ЗаполнитьМатрицуСлучайнымиЗначениями(матрица);

            Console.WriteLine("Исходная матрица:");
            ВывестиМатрицуБайтовНаКонсоль(канал.ПолучитьМатрицу());

            Console.WriteLine("Масштабированная матрица:");
            канал.Масштабирование(4, 4);
            ВывестиМатрицуБайтовНаКонсоль(канал.ПолучитьМатрицу());
        }

        private static void ТестОбратноеМасштабирование()
        {
            Console.WriteLine("Обратно-Масштабированная матрица:");
            канал.ОбратноеМасштабирование(4, 4);
            ВывестиМатрицуБайтовНаКонсоль(канал.ПолучитьМатрицу());
        }

        private static void ТестПеремешатьИСобратьОдинКанал()
        {
            byte[,] матрица1 = new byte[16, 14];
            ЗаполнитьМатрицуСлучайнымиЗначениями(матрица1);
            Канал канал1 = new Канал(матрица1, 2, 2);
            ВывестиМатрицуБайтовНаКонсоль(канал1.ПолучитьМатрицу());

            Канал[] каналы = new Канал[] { канал1 };

            List<Блок> перемешанныеБлоки = JPEG_Cs.JPEG_Cs.Перемешивание(каналы);

            Console.WriteLine("Пустая матрица:");
            byte[,] матрицаКанала = каналы[0].ПолучитьМатрицу();

            for (int j = 0; j < матрицаКанала.GetLength(1); j++)
                for (int i = 0; i < матрицаКанала.GetLength(0); i++)
                    матрицаКанала[i, j] = 0;

            ВывестиМатрицуБайтовНаКонсоль(каналы[0].ПолучитьМатрицу());

            JPEG_Cs.JPEG_Cs.Собрать(каналы, перемешанныеБлоки);

            Console.WriteLine("Собранная матрица:");

            матрицаКанала = каналы[0].ПолучитьМатрицу();
            ВывестиМатрицуБайтовНаКонсоль(матрицаКанала);
        }

        private static void ТестПеремешатьИСобратьМногоКаналов()
        {
            byte[,] матрица1 = new byte[16, 14];
            ЗаполнитьМатрицуСлучайнымиЗначениями(матрица1);
            Канал канал1 = new Канал(матрица1, 2, 2);
            ВывестиМатрицуБайтовНаКонсоль(канал1.ПолучитьМатрицу());

            byte[,] матрица2 = new byte[32, 14];
            ЗаполнитьМатрицуСлучайнымиЗначениями(матрица2);
            Канал канал2 = new Канал(матрица2, 4, 2);
            ВывестиМатрицуБайтовНаКонсоль(канал2.ПолучитьМатрицу());

            byte[,] матрица3 = new byte[16, 28];
            ЗаполнитьМатрицуСлучайнымиЗначениями(матрица3);
            Канал канал3 = new Канал(матрица3, 2, 4);
            ВывестиМатрицуБайтовНаКонсоль(канал3.ПолучитьМатрицу());

            byte[,] матрица4 = new byte[32, 28];
            ЗаполнитьМатрицуСлучайнымиЗначениями(матрица4);
            Канал канал4 = new Канал(матрица4, 4, 4);
            ВывестиМатрицуБайтовНаКонсоль(канал4.ПолучитьМатрицу());

            Канал[] каналы = new Канал[] { канал1, канал2, канал3, канал4 };

            List<Блок> перемешанныеБлоки = JPEG_Cs.JPEG_Cs.Перемешивание(каналы);

            Console.WriteLine("Пустые матрицы:");
            for (int номерКанала = 0; номерКанала < каналы.Length; номерКанала++)
            {
                byte[,] матрицаКанала = каналы[номерКанала].ПолучитьМатрицу();

                for (int j = 0; j < матрицаКанала.GetLength(1); j++)
                    for (int i = 0; i < матрицаКанала.GetLength(0); i++)
                        матрицаКанала[i, j] = 0;

                ВывестиМатрицуБайтовНаКонсоль(каналы[номерКанала].ПолучитьМатрицу());
            }

            JPEG_Cs.JPEG_Cs.Собрать(каналы, перемешанныеБлоки);

            Console.WriteLine("Собранные матрицы:");
            for (int номерКанала = 0; номерКанала < каналы.Length; номерКанала++)
            {
                byte[,] матрицаКанала = каналы[номерКанала].ПолучитьМатрицу();
                ВывестиМатрицуБайтовНаКонсоль(матрицаКанала);
            }
        }

        private static void ЗаполнитьМатрицуКвантования(short[,] матрицаКвантования)
        {
            for (int j = 0; j < матрицаКвантования.GetLength(1); j++)
                for (int i = 0; i < матрицаКвантования.GetLength(0); i++)
                    матрицаКвантования[i, j] = 1;
        }

        public static void ЗаполнитьМатрицуСлучайнымиЗначениями(byte[,] матрица)
        {
            for (int j = 0; j < матрица.GetLength(1); j++)
                for (int i = 0; i < матрица.GetLength(0); i++)
                    матрица[i, j] = (byte)new Random().Next(10, 100);
        }

        private static void ВывестиМатрицуБайтовНаКонсоль(byte[,] матрица)
        {
            for (int j = 0; j < матрица.GetLength(1); j++)
            {
                for (int i = 0; i < матрица.GetLength(0); i++)
                {
                    byte значение = матрица[i, j];
                    if (значение < 10)
                        Console.Write($"00{значение}");
                    else if (матрица[i, j] < 100)
                        Console.Write($"0{значение}");
                    else
                        Console.Write(значение);

                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void ВывестиМатрицуБлоковНаКонсоль(Блок[,] матрицаБлоков)
        {
            for (int j = 0; j < матрицаБлоков.GetLength(1); j++)
            {
                for (int i = 0; i < матрицаБлоков.GetLength(0); i++)
                    ВывестиМатрицуБайтовНаКонсоль(матрицаБлоков[i, j].Элементы);

                Console.WriteLine();
            }
        }

        private static void ВывестиСписокБлоковНаКонсоль(List<Блок> списокБлоков)
        {
            foreach (Блок блок in списокБлоков)
                ВывестиМатрицуБайтовНаКонсоль(блок.Элементы);
        }   

        public static void ТестRGBtoHSV()
        {

            Точка[,] картинка = new Точка[8, 8];
            Точка[,] картинка2 = RGBtoHSV.преобразоватьRGBTOHSV(картинка);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    картинка[i, j].r = 54;
                    картинка[i, j].g = 63;
                    картинка[i, j].b = 99;
                    Console.WriteLine(картинка[i, j].r.ToString() + " " + картинка[i, j].g.ToString() + " " + картинка[i, j].b.ToString());
                    Console.WriteLine(картинка2[i, j].r.ToString() + " " + картинка2[i, j].g.ToString() + " " + картинка2[i, j].b.ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void ТестScan()
        {
            FileStream stream = File.Open("Penguins.jpg", FileMode.Open);
            stream.Seek(0x3c8, SeekOrigin.Begin);
            Scan scanPenguins = JPEGData.ПолучитьДанные(stream) as Scan;
            scanPenguins.Print();
            Console.WriteLine();

            Stream testStream = File.Open("Jellyfish.jpg", FileMode.Open);
            Scan scanJellyfish = new Scan(testStream);
            testStream.Position = 0;
            scanJellyfish.Write();
            testStream.Close();
        }

        public static void ТестQuantizationTable()
        {
            FileStream streamPenguins = File.Open("Penguins.jpg", FileMode.Open);
            streamPenguins.Seek(0x17b, SeekOrigin.Begin);
            QuantizationTable quantPenguins = JPEGData.ПолучитьДанные(streamPenguins) as QuantizationTable;
            quantPenguins.Print();
            streamPenguins.Close();
            Console.WriteLine();

            FileStream streamFish = File.Open("Jellyfish.jpg", FileMode.Open);
            streamFish.Seek(0x17a, SeekOrigin.Begin);
            QuantizationTable quantFish = JPEGData.ПолучитьДанные(streamFish) as QuantizationTable;
            streamFish.Position = 0;
            streamFish.Close();
        }
        public static void TestWrite()
        {
            FileStream streamFish = File.Open("Jellyfish.jpg", FileMode.Open);
            streamFish.Seek(0x17a, SeekOrigin.Begin);
            QuantizationTable quantFish = JPEGData.ПолучитьДанные(streamFish) as QuantizationTable;
            streamFish.Position = 0;
            quantFish.Write();
            streamFish.Close();
        }

        public static void ТестFrame()
        {
            FileStream stream = File.Open("Penguins.jpg", FileMode.Open);
            stream.Seek(0x207, SeekOrigin.Begin);
            Frame penguins = new Frame(ТипМаркера.BaselineDCT_n_dHuffman, stream);
            penguins.Print();
            Console.WriteLine();
        }

        //тест преобразование DC
        public static void ТестРасчетDC()
        {
            Console.WriteLine();
            List<short[,]> blocks = new List<short[,]>();
            short[,] b1 = new short[8, 8];
            short[,] b2 = new short[8, 8];
            short[,] b3 = new short[8, 8];
            short[,] b4 = new short[8, 8];

            Random rand = new Random();
            for (int i = 0; i < b1.GetLength(0); i++)
            {
                for (int j = 0; j < b1.GetLength(1); j++)
                {
                    b1[i, j] += (short)rand.Next(0, 100);
                    b2[i, j] += (short)rand.Next(0, 100);
                    b3[i, j] += (short)rand.Next(0, 100);
                    b4[i, j] += (short)rand.Next(0, 100);
                }
            }

            blocks.Add(b1);
            blocks.Add(b2);
            blocks.Add(b3);
            blocks.Add(b4);

            foreach (short[,] block in blocks)
            {
                for (int i = 0; i < block.GetLength(1) - 1; i++)
                {
                    for (int j = 0; j < block.GetLength(0) - 1; j++)
                    {
                        Console.Write(block[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            int a = 0;
            foreach (short[,] block in blocks)
            {

                if (a != 0)
                {
                    for (int i = 0; i < block.GetLength(1) - 1; i++)
                    {
                        for (int j = 0; j < block.GetLength(0) - 1; j++)
                        {
                            block[i, j + 1] = (short)(block[i, j + 1] - block[i, j]);
                            if (j + 1 == 7)
                            {
                                block[i + 1, 0] = (short)(block[i + 1, 0] - block[i, 7]);//?????????????????????????????
                            }
                        }
                    }
                }
                a += 1;
            }
            foreach (short[,] block in blocks)
            {
                for (int i = 0; i < block.GetLength(1) - 1; i++)
                {
                    for (int j = 0; j < block.GetLength(0) - 1; j++)
                    {
                        Console.Write(block[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            // восстановление
            a = 0;
            foreach (short[,] block in blocks)
            {
                if (a != 0)
                {
                    int pos = 0;
                    int[] D = new int[block.GetLength(1) * block.GetLength(0)];
                    int K = 0;
                    for (int i = 0; i < block.GetLength(0); i++)
                    {
                        for (int j = 0; j < block.GetLength(1); j++)
                        {
                            D[K] = block[i, j];
                            K++;
                        }
                    }

                    for (int c = 0; c < D.Length - 1; c++)
                    {
                        pos += 1;
                    }

                    for (int c = 0; c < D.Length - 1; c++)
                    {
                        D[pos] = D[pos - 1] + D[pos];
                        pos -= 1;
                    }

                    K = 0;
                    for (int i = 0; i < block.GetLength(0); i++)
                    {
                        for (int j = 0; j < block.GetLength(1); j++)
                        {
                            block[i, j] = (short)D[K];
                            K++;
                        }
                    }
                }
                a += 1;
            }
            foreach (short[,] block in blocks)
            {
                for (int i = 0; i < block.GetLength(1) - 1; i++)
                {
                    for (int j = 0; j < block.GetLength(0) - 1; j++)
                    {
                        Console.Write(block[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public static void ТестRestartInterval()
        {
            FileStream stream = File.Open("TestRestartInterval.jpg", FileMode.Open);
            stream.Seek(0x247, SeekOrigin.Begin);
            RestartInterval testRestartInterval = JPEGData.ПолучитьДанные(stream) as RestartInterval;
            testRestartInterval.Print();          
            stream.Close();
        }
        public static void ТестComment()
        {
            FileStream stream = File.Open("TestRestartInterval.jpg", FileMode.Open);
            stream.Seek(0x13c, SeekOrigin.Begin);
            Comment testComment = JPEGData.ПолучитьДанные(stream) as Comment;
            testComment.Print();           
            stream.Close();
	}
	
        public static void ТестHuffmanTable()
        {
            Console.WriteLine();
            FileStream stream = File.Open("Penguins.jpg", FileMode.Open);
            stream.Seek(0x1E71, SeekOrigin.Begin);
            HuffmanTable тестHuffmanTable = new HuffmanTable(stream);
            тестHuffmanTable.Print();
            stream.Close();
        }

        private static void ТестJPEGFile()
        {
            FileStream streamPenguins = File.Open("Penguins.jpg", FileMode.Open);
            JPEGFile file = new JPEGFile(streamPenguins);
            file.Print();
        }

        static void ТестКанал1()
        {
            byte[,] matrix1 = new byte[17, 6];
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    matrix1[i, j] = (byte)(i + j);
                }
            }
            Console.WriteLine("Вывод изначальной матрицы");
            ВывестиМатрицуБайтовНаКонсоль(matrix1);
            Канал channel = new Канал(matrix1, 2, 1);
            channel.Дополнение(2, 2);
            Console.WriteLine("Вывод матрицы после дополнения");
            ВывестиМатрицуБайтовНаКонсоль(channel.ПолучитьМатрицу());
            channel.Масштабирование(2, 2);
            Console.WriteLine("Вывод матрицы после масштабирования");
            ВывестиМатрицуБайтовНаКонсоль(channel.ПолучитьМатрицу());
            List<Блок> блоки = channel.РазбитьМатрицуНаБлокиВФормеСписка(8, 8);
            Console.WriteLine("Вывод блоков");
            foreach (var блок in блоки)
            {               
                byte[,] matrix2 = блок.Элементы;
                ВывестиМатрицуБайтовНаКонсоль(matrix2);
                Console.WriteLine();
            }
            channel = new Канал(matrix1, 2, 1);
            channel.Дополнение(2, 2);
            Console.WriteLine("Вывод изначальной матрицы");
            ВывестиМатрицуБайтовНаКонсоль(channel.ПолучитьМатрицу());
            channel.Собрать(блоки, 2, 2);
            Console.WriteLine("Вывод матрицы после сборки");
            ВывестиМатрицуБайтовНаКонсоль(channel.ПолучитьМатрицу());
            channel.ОбратноеМасштабирование(2, 2);
            Console.WriteLine("Вывод матрицы после обратного масштабирования");
            ВывестиМатрицуБайтовНаКонсоль(channel.ПолучитьМатрицу());
        }

        public static void ПроверкаЗигзага()
        {
            Console.WriteLine();
            Console.Write("Входная матрица:");
            Console.WriteLine();
            short[,] matrix = new short[8, 8] { { 0, 2, 3, 9, 10, 20, 21, 35 }, { 1, 4, 8, 11, 19, 22, 34, 36 }, { 5, 7, 12, 18, 23, 33, 37, 48 }, { 6, 13, 17, 24, 32, 38, 47, 49 }, { 14, 16, 25, 31, 39, 46, 50, 57 }, { 15, 26, 30, 40, 45, 51, 56, 58 }, { 27, 29, 41, 44, 52, 55, 59, 62 }, { 28, 42, 43, 53, 54, 60, 61, 63 } };
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(matrix[j, i] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.Write("Зигзаг:");
            Console.WriteLine();
            short[] matrix1 = DCT.Зигзаг(matrix);
            for (int i = 0; i < matrix1.Length; i++)
            {
                Console.Write(matrix1[i] + " ");
            }
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Обратный зигзаг:");
            Console.WriteLine();
            short[,] matrix2 = DCT.ОбратныйЗигзаг(matrix1);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(matrix2[j, i] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
