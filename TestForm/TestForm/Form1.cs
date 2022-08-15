using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JPEG_Cs;
using System.IO;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Unbox();
            YUVtoRGBTest();
        }

        public void Unbox()
        {
            JPEG_Cs.JPEG_Cs jp = new JPEG_Cs.JPEG_Cs(File.Open("Penguins.jpg", FileMode.Open));
            Точка[,] points = jp.Распаковать();
            Bitmap bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < points.GetLength(1); j++)
                {
                    Color pColor = Color.FromArgb(points[i,j].r, points[i,j].g, points[i,j].b);
                    bit.SetPixel(i, j, pColor);
                }
            }
            pictureBox1.Image = bit;
        }

        public void YUVtoRGBTest()
        {
            JPEG_Cs.JPEG_Cs jp = new JPEG_Cs.JPEG_Cs(File.Open("Penguins.jpg", FileMode.Open));
            Точка[,] цвета = jp.Распаковать();
            Bitmap image1;
            image1 = new Bitmap(200, 100);

            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    Color pixelColor = image1.GetPixel(i, j);
                    Color newColor = Color.FromArgb(цвета[i, j].r, цвета[i, j].g, цвета[i, j].b);
                    image1.SetPixel(i, j, newColor);
                }
            }

            pictureBox1.Image = image1;
        }
    }
}
