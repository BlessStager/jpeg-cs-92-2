using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JPEG_Cs
{
    /// <summary>
    /// Хранит параметры таблицы квантования.
    /// </summary>
    public class QuantizationTable : JPEGData
    {
        /// <summary>
        /// Определяет точность значений таблицы квантования. Значение 0 указывает на 8-битные Qk значения; значение 1 указывает на 16-битные Qk значения.
        /// </summary>
        public byte Pq { get; set; } //4 бита
        /// <summary>
        /// Указывает на одно из четырёх возможных мест назначения декодера, в которое должна быть установлена таблица квантования.
        /// </summary>
        public byte Tq { get; set; } //4 бита
        /// <summary>
        /// Задает k-ый элемент из 64, где k - индекс в зигзагообразном порядке коэффициентов DCT.
        /// </summary>
        public byte[] Qk { get; set; } = new byte[64];
        /// <summary>
        /// Читает таблицу квантования из потока, начиная с 3-го поля.
        /// </summary>
        public QuantizationTable(Stream s) : base(ТипМаркера.DefquatTab, s)
        {
            byte pq, tq;
            Read4(out pq, out tq);
            Pq = pq;
            Tq = tq;
            for (int i = 0; i < Qk.Length; i++)
            {
                Qk[i] = (byte)s.ReadByte();
            }
        }
        /// <summary>
        /// Пишет таблицу квантования в поток.
        /// </summary>
        public override void Write()
        {
            base.Write();
            Write4(Pq, Tq);
            for (int i = 0; i < Qk.Length; i++)
            {
                stream.WriteByte(Qk[i]);
            }
        }
        /// <summary>
        /// Выводит на консоль значения параметров таблицы квантования.
        /// </summary>
        public override void Print()
        {
            base.Print();
            Console.WriteLine("Pq = {0:X}", Pq);
            Console.WriteLine("Tq = {0:X}", Tq);
            for (int i = 0; i < Qk.Length; i++)
            {
                Console.WriteLine("Значение параметра Q{0} = {1:X}",i, Qk[i]);
            }
        }
    }
}
