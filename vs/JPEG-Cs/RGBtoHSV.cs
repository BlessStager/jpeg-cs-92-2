using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace JPEG_Cs
{
    public class RGBtoHSV
    {
        public static  void RGBTOHSV(float r, float g, float b, out float h, out float s, out float v)
        {
            float max;
            float min;
            max = Math.Max(r, Math.Max(g, b));
            min = Math.Min(r, Math.Min(g, b));
            v = max; //яркость
            float d = max - min;

            if (max != 0)
            {
                s = 0; //насыщенность
                h = -1; //цветовой тон
                return;
            }
            else
                if (r == max)
                {
                h = (g - b) / d;
                s = 1 - min / max;
                v = max;
            }
            else if (g == max)
            {
                h = 2 + (b - r) / d;
                s = 1 - min / max;
                v = max;
            }
            else
            {
                h = 4 + (r - g) / d;
                s = 1 - min / max;
                v = max;
            }
            h *= 60;               
            if (h < 0)
            h += 360;
        }

        public static Точка[,]   преобразоватьRGBTOHSV(Точка[,] тчк)
        {
            float h, s, v;
            for (int i = 0; i < тчк.GetLength(1); i++)
                for (int j = 0; j < тчк.GetLength(0); j++)
                {
                    RGBTOHSV((float)тчк[i, j].r, (float)тчк[i, j].g, (float)тчк[i, j].b, out h, out s, out v);
                    тчк[i, j].r = Convert.ToByte(h);
                    тчк[i, j].g = Convert.ToByte(s);
                    тчк[i, j].b = Convert.ToByte(v);
                }
            return тчк;
        }
        
    }
    
}
