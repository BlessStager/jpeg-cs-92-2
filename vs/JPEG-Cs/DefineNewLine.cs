using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JPEG_Cs
{

    public class DefineNewLine: JPEGData
    {
        /// <summary>
        /// Количество строк
        /// </summary>
        protected ushort высота;

        /// <summary>
        /// конструктор класса DefineNewLine
        /// </summary>
        /// <param name="s"></param>
        public DefineNewLine(Stream s): base(ТипМаркера.Defnumoflines, s)
        {
            высота = (ushort)s.ReadByte();
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine("Количество строк :" + высота.ToString());
        }
    }
}
