using System;
using System.IO;

namespace JPEG_Cs
{
    /// <summary>
    /// Таблицы кодов Хаффмана
    /// </summary>
    public class HuffmanTable : JPEGData
    {
        /// <summary>
        /// массив сколько кодов данной длины(от 1 до 16)
        /// </summary>
        private byte[] codeLenght = new byte[16];

        /// <summary>
        /// значения кодов
        /// </summary>
        private byte[] values;

        /// <summary>
        /// Список длин кодов
        /// </summary>
        private byte[] huffSize;
        private int lastK;

        /// <summary>
        /// Коды, соответствующие длинам
        /// </summary>
        private ushort[] huffCode;

        /// <summary>
        /// Отсортированные по значениям длины кодов
        /// </summary>
        private ushort[] ehufSi;

        /// <summary>
        /// Отсортированные по значениям коды
        /// </summary>
        private ushort[] ehufCo;

        /// <summary>
        /// TC - класс таблицы (0 - DC, 1 - AC)
        /// </summary>
        private byte TC;

        /// <summary>
        /// номер таблицы
        /// </summary>
        private byte TH;

        /// <summary>
        /// читает таблицу Хаффмана из потока начиная с 3-го поля
        /// </summary>
        /// <param name="s"></param>
        public HuffmanTable(Stream s) : base(ТипМаркера.DefineHuffmantebles, s)
        {
            byte post = (byte)stream.ReadByte();
            TC = (byte)(post >> 4);
            TH = (byte)(post & 0x0F);
            byte сумма = 0;
            for (int i = 0; i < 16; i++)
            {
                codeLenght[i] = (byte)stream.ReadByte();
                сумма += codeLenght[i];
            }
            values = new byte[сумма];
            for (int i = 0; i < сумма; i++)
            {
                values[i] = (byte)stream.ReadByte();
            }

            СоздатьHuffSize();
            СоздатьHuffCode();
            Создать_ehufSi_и_ehufCo();
        }

        private void СоздатьHuffSize()
        {
            huffSize = new byte[values.Length];
            int k = 0;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < codeLenght[i]; j++)
                {
                    huffSize[k] = (byte)(i + 1);
                    k++;
                }
            }
            huffSize[k-1] = 0;
            lastK = k;
        }

        private void СоздатьHuffCode()
        {
            huffCode = new ushort[huffSize.Length];
            int k = 0;
            ushort code = 0;
            byte si = huffSize[0];
            do
            {
                huffCode[k] = code;
                code++;
                k++;

                if (huffSize[k] == si)
                    continue;
                else
                {
                    if (huffSize[k] == 0)
                        break;

                    else
                    {
                        do
                        {
                            code = (ushort)(code << 1);
                            si++;
                        } while (huffSize[k] != si);
                    }
                }
            } while (true);
        }

        private void Создать_ehufSi_и_ehufCo()
        {
            ehufCo = new ushort[256];
            ehufSi = new ushort[256];
            for (int k = 0; k < lastK; k++)
            {
                var i = values[k];
                ehufCo[i] = huffCode[k];
                ehufSi[i] = huffSize[k];
            }
        }

        /// <summary>
        /// пишет таблицу Хаффмана в поток начиная с 3-го поля
        /// </summary>
        public override void Write()
        {
            base.Write();
            stream.WriteByte((byte)((TC << 4) + TH));
            foreach (byte i in codeLenght)
            {
                stream.WriteByte(i);
            }
            foreach (byte i in values)
            {
                stream.WriteByte(i);
            }
        }

        /// <summary>
        /// печатает все поля таблицы Хаффмана
        /// </summary>
        public override void Print()
        {
            base.Print();
            Console.WriteLine("TC :" + TC + " " + "TH :" + TH);
            Console.WriteLine("Длина кодов :");
            foreach (byte i in codeLenght)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine(Environment.NewLine + "Значения :");
            foreach (byte i in values)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            Console.WriteLine(Environment.NewLine + "huffSize:");
            foreach (byte i in huffSize)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine(Environment.NewLine + "huffCode:");
            foreach (ushort i in huffCode)
            {
                Console.Write(Convert.ToString(i, 2) + " ");
            }
            Console.WriteLine(Environment.NewLine + "ehufSi:");
            foreach (ushort i in ehufSi)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine(Environment.NewLine + "ehufCo:");
            foreach (ushort i in ehufCo)
            {
                Console.Write(Convert.ToString(i, 2) + " ");
            }
            Console.WriteLine();
        }
    }

}
