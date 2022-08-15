using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JPEG_Cs
{
    /// <summary>
    /// Декодирование энтропийно закодированных данных
    /// </summary>
    public class Decoding
    {
        byte B = 0xFF;
        byte B2;
        int CNT = 0;
        Stream stream;

        /// <summary>
        /// Cоздает объект, сохраняет поток в классе
        /// </summary>
        /// <param name="s">Сохраняемый поток</param>
        public Decoding(Stream s, HuffmanTable huffDC, HuffmanTable huffAC)
        {
            stream = s;
        }
        /// <summary>
        /// Читает следующий бит из сжатых данных, распознает маркеры и stuff байты
        /// </summary>
        public byte NextBit()
        {
            byte BIT;

            if (CNT == 0)
            {
                B = (byte)stream.ReadByte();
                CNT = 8;
                if (B == 0xFF)
                {
                    B2 = (byte)stream.ReadByte();
                    if (B2 != 0)
                    {
                        if (B2 == 0xDC)
                        {                           
                            throw new DNLMarkerException("DNLMarkerException");
                        } 
                        else 
                        {
                            throw new Exception("Error");
                        }
                    }
                }
            }
            BIT = (byte)(B >> 7);
            CNT--;
            B = (byte)(B << 1);
            return BIT;
        }

        /// <summary>
        /// Конвертирует частично декодированную разницу DC коэффициентов в полный код (расширение знакового бита в коде)
        /// </summary>
        /// <param name="diff">Частичный код разницы DC</param>
        /// <param name="num_bits">Число бит для разницы</param>
        /// <returns>Декодированная DC разница</returns>
        public short Extend(ushort diff, int num_bits)
        {
            short diffT = (short)Math.Pow(2, num_bits - 1);
            short result = (short)diff;
            if (diff < diffT)
            {
                diffT = (short)((-1 << num_bits) + 1);
                result = (short)(diff + diffT);
            }
            return result;
        }
    }
}
