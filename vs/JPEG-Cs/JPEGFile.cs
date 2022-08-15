using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JPEG_Cs
{
    /// <summary>
    /// Работа с файлом JPEG
    /// </summary>
    public class JPEGFile
    {
        /// <summary>
        /// Список всех структур JPEG до StartOfScan.
        /// </summary>
        List<JPEGData> данные = new List<JPEGData>();

        /// <summary>
        /// Поток для данных
        /// </summary>
        Stream поток;

        /// <summary>
        /// Кадр
        /// </summary>
        Frame frame;

        /// <summary>
        /// Все таблицы квантования
        /// </summary>
        List<QuantizationTable> таблицыКвантования = new List<QuantizationTable>();

        /// <summary>
        /// Все таблицы Хаффмана
        /// </summary>
        List<HuffmanTable> таблицыХаффмана = new List<HuffmanTable>();

        /// <summary>
        /// Заголовок кодированных данных
        /// </summary>
        Scan scan;

        /// <summary>
        /// Интервал повтора
        /// </summary>
        RestartInterval restartInterval;

        /// <summary>
        /// Класс декодирования энтропийных данных.
        /// </summary>
        Decoding декодирование;

        /// <summary>
        /// Конструктор JPEGFile. Считывает все структуры JPEGData из потока
        /// </summary>
        /// <param name="stream">Поток с изображением.</param>
        public JPEGFile(Stream stream)
        {
            поток = stream;
            while (true)
            {
                long позиция = поток.Position + 2;
                JPEGData data = JPEGData.ПолучитьДанные(поток);

                if (data is Scan)
                {
                    данные.Add((Scan)data);
                    scan = (Scan)data;
                    break;
                }
                else if (data is HuffmanTable)
                {
                    данные.Add((HuffmanTable)data);
                    таблицыХаффмана.Add((HuffmanTable)data);
                }
                else if (data is Frame)
                {
                    данные.Add((Frame)data);
                    frame = (Frame)data;
                }
                else if (data is QuantizationTable)
                {
                    данные.Add((QuantizationTable)data);
                    таблицыКвантования.Add((QuantizationTable)data);
                }
                else if (data is RestartInterval)
                {
                    данные.Add((RestartInterval)data);
                    restartInterval = (RestartInterval)data;
                }
                else
                {
                    данные.Add(data);
                }
                поток.Position = позиция + data.длина;
            }
            декодирование = new Decoding(поток, null, null);
        }

        /// <summary>
        /// Выводит в консоль все структуры из спиcка, и отдельно списки таблиц квантования и Хаффмана.
        /// </summary>
        public void Print()
        {            
            foreach (JPEGData data in данные)
            {
                data.Print();
                Console.WriteLine("");
                Console.WriteLine("");
            }
            foreach (QuantizationTable data in таблицыКвантования)
            {
                data.Print();
                Console.WriteLine("");
                Console.WriteLine("");
            }

            frame.Print();
            Console.WriteLine("");
            Console.WriteLine("");

            foreach (HuffmanTable data in таблицыХаффмана)
            {
                data.Print();
                Console.WriteLine("");
                Console.WriteLine("");
            }

            if (restartInterval != null)
                restartInterval.Print();

            scan.Print();
        }

        /// <summary>
        /// Декодирование кадра JPEG
        /// </summary>
        /// <returns>Массив масштабированных каналов изображения</returns>
        public Канал[] DecodeFrame()
        {
            Канал[] каналы = new Канал[3];
            byte[,] БлокY = new byte[200, 100];
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 50; j++)
                {
                    БлокY[i, j] = 0;
                }
            for (int i = 100; i < 200; i++)
                for (int j = 0; j < 50; j++)
                {
                    БлокY[i, j] = 64;
                }
            for (int i = 0; i < 100; i++)
                for (int j = 50; j < 100; j++)
                {
                    БлокY[i, j] = 128;
                }
            for (int i = 100; i < 200; i++)
                for (int j = 50; j < 100; j++)
                {
                    БлокY[i, j] = 255;
                }
            Канал каналY = new Канал(БлокY, 200, 100);
            byte[,] БлокU = new byte[200, 100];
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 50; j++)
                {
                    БлокU[i, j] = 0;
                }
            for (int i = 100; i < 200; i++)
                for (int j = 0; j < 50; j++)
                {
                    БлокU[i, j] = 64;
                }
            for (int i = 0; i < 100; i++)
                for (int j = 50; j < 100; j++)
                {
                    БлокU[i, j] = 128;
                }
            for (int i = 100; i < 200; i++)
                for (int j = 50; j < 100; j++)
                {
                    БлокU[i, j] = 255;
                }
            Канал каналU = new Канал(БлокU, 200, 100);
            byte[,] БлокV = new byte[200, 100];
            for (int i = 0; i < 100; i++)
                for (int j = 0; j < 50; j++)
                {
                    БлокV[i, j] = 255;
                }
            for (int i = 100; i < 200; i++)
                for (int j = 0; j < 50; j++)
                {
                    БлокV[i, j] = 128;
                }
            for (int i = 0; i < 100; i++)
                for (int j = 50; j < 100; j++)
                {
                    БлокV[i, j] = 64;
                }
            for (int i = 100; i < 200; i++)
                for (int j = 50; j < 100; j++)
                {
                    БлокV[i, j] = 0;
                }
            Канал каналV = new Канал(БлокV, 200, 100);
            каналы[0] = каналY;
            каналы[1] = каналU;
            каналы[2] = каналV;
            return каналы;
        }
    }
}
