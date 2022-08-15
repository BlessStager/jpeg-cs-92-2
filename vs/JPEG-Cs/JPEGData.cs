using System;
using System.IO;

namespace JPEG_Cs
{
    /// <summary>
    /// Класс для работы со структурами данных JPEG
    /// </summary>
    public class JPEGData
    {
        /// <summary>
        /// Поток JPEG.
        /// </summary>
        protected Stream stream;

        /// <summary>
        /// Тип маркера.
        /// </summary>
        protected ТипМаркера тип;

        /// <summary>
        /// Длина структуры.
        /// </summary>
        public ushort длина;

        /// <summary>
        /// Инициализирует поток для работы с методами класса.
        /// </summary>
        public JPEGData(ТипМаркера тип, Stream s)
        {
            this.тип = тип;
            stream = s;
            if ((тип < ТипМаркера.Restartwithmodulo8countm0) && (тип != ТипМаркера.FortempprivuseianArithcoding) || (тип > ТипМаркера.EndofImage))
            {
                длина = Read16();
            }
        }

        /// <summary>
        /// Считывает два четырёхбитных значения, первое - старшие 4 бита, второе - младшие. 
        /// </summary>
        public void Read4(out byte p1, out byte p2)
        {
            int curByte = stream.ReadByte();
            p1 = (byte)(curByte >> 4);
            p2 = (byte)(curByte & 0xf);
        }

        /// <summary>
        /// Записывает два четырёхбитных значения.
        /// </summary>
        public void Write4(byte p1, byte p2)
        {
            p1 = (byte)(p1 << 4);
            byte result = (byte)(p1 + p2);
            stream.WriteByte(result);      
        }

        /// <summary>
        /// Считывает из потока двухбайтовое значение и возвращает его, начиная со старшего байта.
        /// </summary>
        /// /// <returns>
        /// Возвращает это двухбайтовое значение.
        /// </returns>
        public ushort Read16()
        {
            byte b1 = (byte)stream.ReadByte();
            byte b2 = (byte)stream.ReadByte();
            return (ushort)((b1 << 8) + b2);
        }

        /// <summary>
        /// Считывает из потока двухбайтовое значение и возвращает его, начиная со старшего байта.
        /// </summary>
        /// /// <returns>
        /// Возвращает это двухбайтовое значение.
        /// </returns>
        static public ushort Read16(Stream curStream)
        {
            byte b1 = (byte)curStream.ReadByte();
            byte b2 = (byte)curStream.ReadByte();
            return (ushort)((b1 << 8) + b2);
        }

        /// <summary>
        /// Записывает в текущий поток двухбайтовое значение.
        /// </summary>
        public void Write16(ushort data)
        {
            byte b1 = (byte)(data >> 8);
            byte b2 = (byte)(data & 0xff);
            stream.WriteByte(b1);
            stream.WriteByte(b2);
        }

        /// <summary>
        /// Записывает в текущий поток тип маркера и длину.
        /// </summary>
        public virtual void Write()
        {
            Write16((ushort)тип);
            if ((тип < ТипМаркера.Restartwithmodulo8countm0) && (тип != ТипМаркера.FortempprivuseianArithcoding) || (тип > ТипМаркера.EndofImage))
            {
                Write16(длина);
            }          
        }

        /// <summary>
        /// Читает маркер и в зависимости от типа маркера создаёт нужную структуру данных JPEG.
        /// </summary>
        /// /// /// <returns>
        /// Возвращает экземпляр соответствующего класса.
        /// </returns>
        static public JPEGData ПолучитьДанные(Stream s)
        {
            ТипМаркера тип = (ТипМаркера)Read16(s);
            if (тип == ТипМаркера.StartofSkan) return new Scan(s);
            else if (тип == ТипМаркера.DefquatTab) return new QuantizationTable(s);
            else if (тип == ТипМаркера.BaselineDCT_n_dHuffman || тип == ТипМаркера.ExtendedSeqDCT_n_dHuffman 
                || тип == ТипМаркера.ProgressiveDCT_n_dHuffman || тип == ТипМаркера.Lossless_n_dHuffman 
                || тип == ТипМаркера.DifseqDCT_dHuffman || тип == ТипМаркера.DifprogDCT_dHuffman 
                || тип == ТипМаркера.DiflosslessDCT_dHuffman || тип == ТипМаркера.ResforJPEGext_n_dArithmetic 
                || тип == ТипМаркера.ExtendedSeqDCT_n_dArithmetic || тип == ТипМаркера.ProgressiveDCT_n_dArithmetic 
                || тип == ТипМаркера.Lossless_n_dArithmetic) return new Frame(тип, s);
            else if (тип == ТипМаркера.DefineHuffmantebles) return new HuffmanTable(s);
            else if (тип == ТипМаркера.Defresint) return new RestartInterval(s);
            else if (тип == ТипМаркера.Comment) return new Comment(s);
            else if ((int)тип >= 0xFFE0 && (int)тип <= 0xFFEF) return new AppData(s);
            else if (тип == ТипМаркера.Defnumoflines) return new DefineNewLine(s);
            else return new JPEGData(тип, s);
        }

        /// <summary>
        /// Выводит на консоль тип маркера и длину.
        /// </summary>
        public virtual void Print()
        {
            Console.WriteLine("{0:X} - {1}", (ushort)тип, тип);
            Console.WriteLine("Длина: " + длина);
        }
    }
}
