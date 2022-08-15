using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JPEG_Cs
{
    /// <summary>
    /// Данные приложения, наследуется от JPEGData. Раздел B.2.4.6
    /// </summary>
    class AppData : JPEGData
    {
        /// <summary>
        /// Массив данных
        /// </summary>
        public byte[] data;

        /// <summary>
        /// Создает структуру, читает данные, длинной Length - 2
        /// </summary>
        /// <param name="s"></param>
        public AppData(Stream s) : base(ТипМаркера.ReservedForAppSegment, s)
        {
            data = new byte[длина - 2];
            for (int i = 0; i < длина - 2; i++)
                data[i] = (byte)s.ReadByte();
        }

        /// <summary>
        /// Пишет данные в поток
        /// </summary>
        public override void Write()
        {
            base.Write();
            for (int i = 0; i < data.Length; i++)
                stream.WriteByte(data[i]);
        }

        /// <summary>
        /// Печатает данные
        /// </summary>
        public override void Print()
        {
            base.Print();
            Console.Write("Массив AppData: ");
            for (int i = 0; i < data.Length; i++)
                Console.Write($"{data[i]}");
        }
    }
}
