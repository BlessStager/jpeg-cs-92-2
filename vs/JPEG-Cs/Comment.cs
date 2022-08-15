using System;
using System.IO;

namespace JPEG_Cs
{
    /// <summary>
    /// Хранит комментарий в JPEG файле.
    /// </summary>
    public class Comment : JPEGData
    {
        /// <summary>
        /// Текст комментария.
        /// </summary>
        public byte[] comment { get; set; }

        /// <summary>
        /// Читает комментарий длиной Length - 2
        /// </summary>
        public Comment(Stream s) : base(ТипМаркера.Comment, s)
        {
            comment = new byte[длина - 2];
            for (int i = 0; i < comment.Length; i++)
            {
                comment[i] = (byte)stream.ReadByte();
            }
        }

        /// <summary>
        /// Пишет комментарий в поток.
        /// </summary>
        public override void Write()
        {
            base.Write();
            for (int i = 0; i < comment.Length; i++)
            {
                stream.WriteByte(comment[i]);
            }
        }

        /// <summary>
        /// Печатает комментарий.
        /// </summary>
        public override void Print()
        {
            base.Print();
            Console.Write("Массив comment: ");
            for (int i = 0; i < comment.Length; i++)
            {
                Console.Write("{0:X} ", comment[i]);
            }
            Console.WriteLine();
            Console.Write("Текст комментария: ");
            for (int i = 0; i < comment.Length; i++)
            {
                Console.Write(Convert.ToChar(comment[i]));
            }
            Console.WriteLine();
        }
    }
}
