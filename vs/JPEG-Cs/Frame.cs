using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JPEG_Cs
{

    /// <summary>
    /// Содержит параметры изображения
    /// </summary>
    public class Frame : JPEGData
    {
        /// <summary>
        /// Параметры канала
        /// </summary>
        struct Компонент
        {
            /// <summary>
            /// Номер компоненты
            /// </summary>
            public byte номер;
            /// <summary>
            /// Коэффициент горизонтальной выборки
            /// </summary>
            public byte H;
            /// <summary>
            /// Коэффициент вертикальной выборки
            /// </summary>
            public byte V;
            /// <summary>
            /// Номер таблицы квантования
            /// </summary>
            public byte номер_таблицы_квантования;
        }

        /// <summary>
        /// Точность представления в битах
        /// </summary>
        byte число_бит;

        /// <summary>
        /// Число колонок в исходном изображении
        /// </summary>
        Int16 ширина;

        /// <summary>
        /// Число строк в исходном изображении
        /// </summary>
        Int16 высота;

        /// <summary>
        /// Число каналов в изображении
        /// </summary>
        byte число_компонент;

        /// <summary>
        /// Массив комонент
        /// </summary>
        Компонент[] компоненты;

        /// <summary>
        /// Конструктор класса Frame
        /// </summary>
        /// <param name="s"></param>        
        public Frame(ТипМаркера типМаркера, Stream s) : base(типМаркера, s)
        {
            число_бит = (byte)s.ReadByte();
            высота = (short)Read16(s);
            ширина = (short)Read16(s);
            число_компонент = (byte)s.ReadByte();
            компоненты = new Компонент[число_компонент];
            for (int i = 0; i < (int)число_компонент; i++)
            {
                компоненты[i].номер = (byte)s.ReadByte();
                Read4(out компоненты[i].H, out компоненты[i].V);
                компоненты[i].номер_таблицы_квантования = (byte)s.ReadByte();
            }
        }

        /// <summary>
        /// Выводит в консоль значаения параметров фрейма
        /// </summary>
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Число компонент = {0:X}", число_компонент);
            for (int i = 0; i < число_компонент; i++)
            {
                Console.WriteLine("Номер компоненты = {0:X}", компоненты[i].номер);
                Console.WriteLine("H = {0:X}", компоненты[i].H);
                Console.WriteLine("V = {0:X}", компоненты[i].V);
                Console.WriteLine("Номер таблицы квантования = {0:X}", компоненты[i].номер_таблицы_квантования);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Чтение числа бит
        /// </summary>
        public Int16 Число_бит
        {
            get { return число_бит; }
        }

        /// <summary>
        /// Чтение ширины
        /// </summary>
        public Int16 Ширина
        {
            get { return ширина; }
        }

        /// <summary>
        /// Чтение высоты
        /// </summary>
        public Int16 Высота
        {
            get { return высота; }
        }

        /// <summary>
        /// Чтение числа компонент
        /// </summary>
        public Int16 Число_компонент
        {
            get { return число_компонент; }
        }
    }
}
