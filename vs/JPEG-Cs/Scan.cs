using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JPEG_Cs
{
    /// <summary>
    /// Хранит параметры закодированных данных
    /// </summary>
    public class Scan : JPEGData
    {

        public struct Компонент
        {
            public byte номер;
            public byte DCтаблицаХаффмана; //4 бита
            public byte ACтаблицаХаффмана; //4 бита
        }
        /// <summary>
        /// Хранит количество деталей изображения при сканировании.
        /// </summary>
        public byte ЧислоКомпонент { get; set; }
        /// <summary>
        /// Хранит набор компонентов изображения при сканировании.
        /// </summary>
        public Компонент[] компоненты { get; set; }
        /// <summary>
        /// Хранит первый коэффициент DCT в каждом блоке, который должен быть закодирован в зигзагообразном порядке.
        /// </summary>
        public byte первыйКоэффициент { get; set; }
        /// <summary>
        /// Хранит последний коэффициент DCT в каждом блоке, который должен быть закодирован в зигзагообразном порядке.
        /// </summary>
        public byte последнийКоэффициент { get; set; }
        /// <summary>
        /// Определяет точечное преобразование, используемое в предыдущем сканировании для диапазона коэффициентов, заданного первыйКоэффициент и последнийКоэффициент.
        /// </summary>
        public byte Ah { get; set; }
        /// <summary>
        /// Определяет точечное преобразование, используемое перед кодированием диапазона коэффициентов, заданного первыйКоэффициент и последнийКоэффициент.
        /// </summary>
        public byte Al { get; set; }

        /// <summary>
        /// Читает скан из потока, начиная с 3-го поля.
        /// </summary>
        public Scan(Stream s) : base(ТипМаркера.StartofSkan, s)
        {
            ЧислоКомпонент = (byte)stream.ReadByte();
            компоненты = new Компонент[ЧислоКомпонент];
            for (int i = 0; i < ЧислоКомпонент; i++)
            {
                компоненты[i].номер = (byte)stream.ReadByte();
                Read4(out компоненты[i].DCтаблицаХаффмана, out компоненты[i].ACтаблицаХаффмана);
            }
            первыйКоэффициент = (byte)stream.ReadByte();
            последнийКоэффициент = (byte)stream.ReadByte();
            byte ah;
            byte al;
            Read4(out ah, out al);
            Ah = ah;
            Al = al;
        }
        /// <summary>
        /// Пишет скан в поток.
        /// </summary>
        public override void Write()
        {
            base.Write();
            stream.WriteByte(ЧислоКомпонент);
            for (int i = 0; i < ЧислоКомпонент; i++)
            {
                stream.WriteByte(компоненты[i].номер);
                Write4(компоненты[i].DCтаблицаХаффмана, компоненты[i].ACтаблицаХаффмана);
            }
            stream.WriteByte(первыйКоэффициент);
            stream.WriteByte(последнийКоэффициент);
            Write4(Ah, Al);
        }
        /// <summary>
        /// Выводит на консоль значения параметров скана.
        /// </summary>
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Число компонент = {0:X}", ЧислоКомпонент);
            for (int i = 0; i < ЧислоКомпонент; i++)
            {
                Console.WriteLine("Параметры {0} компоненты:", i + 1);
                Console.WriteLine("Номер компоненты = {0:X}", компоненты[i].номер);
                Console.WriteLine("DCтаблицаХаффмана компоненты = {0:X}", компоненты[i].DCтаблицаХаффмана);
                Console.WriteLine("ACтаблицаХаффмана компоненты = {0:X}", компоненты[i].ACтаблицаХаффмана);
            }
            Console.WriteLine("Первый коэффициент = {0:X}", первыйКоэффициент);
            Console.WriteLine("Последний коэффициент = {0:X}", последнийКоэффициент);
            Console.WriteLine("Ah = {0:X}", Ah);
            Console.WriteLine("Al = {0:X}", Al);
        }
    }
}
