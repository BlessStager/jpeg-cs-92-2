using System;
using System.IO;

namespace JPEG_Cs
{
    /// <summary>
    /// Хранит параметр интервала повтора.
    /// </summary>
    public class RestartInterval : JPEGData
    {
        /// <summary>
        /// Задаёт количество MCU в интервале повтора.
        /// </summary>
        public ushort restartInterval { get; set; }
        /// <summary>
        /// Читает интервал повтора из потока.
        /// </summary>
        public RestartInterval(Stream s) : base(ТипМаркера.Defresint, s)
        {
            restartInterval = Read16();
        }
        /// <summary>
        /// Пишет интервал повтора в поток.
        /// </summary>
        public override void Write()
        {
            base.Write();
            Write16(restartInterval);
        }
        /// <summary>
        /// Выводит на консоль значение параметра интервала повтора.
        /// </summary>
        public override void Print()
        {
            base.Print();
            Console.WriteLine("restartInterval = {0:X}", restartInterval);
        }
    }
}
