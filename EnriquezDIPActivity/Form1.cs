using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnriquezDIPActivity
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        Bitmap imageB, imageA;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Title = "Save Image";
            saveFileDialog1.DefaultExt = "jpg";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (pictureBox3 == null)
                pictureBox2.Image.Save(saveFileDialog1.FileName);
            else
                pictureBox3.Image.Save(saveFileDialog1.FileName);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = loaded;
        }

        private void greyscalingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded);
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color p = processed.GetPixel(i, j);
                    int grey = (p.R + p.G + p.B) / 3;
                    Color greyScale = Color.FromArgb(grey, grey, grey);
                    processed.SetPixel(i, j, greyScale);
                }
            }
            pictureBox2.Image = processed;
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded);
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color p = loaded.GetPixel(i, j);
                    processed.SetPixel((loaded.Width - 1) - i, j, p);
                }
            }
            pictureBox2.Image = processed;
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded);
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color p = loaded.GetPixel(i, j);
                    processed.SetPixel(i, (loaded.Height - 1) - j, p);
                }
            }
            pictureBox2.Image = processed;
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded);
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color p = loaded.GetPixel(i, j);
                    processed.SetPixel(i, j, Color.FromArgb(255 - p.R, 255 - p.G, 255 - p.B));
                }
            }
            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] histogram = new int[256];
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color p = loaded.GetPixel(i, j);
                    int avg = (p.R + p.G + p.B) / 3;
                    histogram[avg]++;
                }
            }

            int max = histogram.Max();
            int scale = 100;
            int width = 256;
            int height = 100;

            Bitmap histogramImage = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (j < (histogram[i] * scale / max))
                    {
                        histogramImage.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        histogramImage.SetPixel(i, j, Color.White);
                    }
                }
            }
            pictureBox2.Image = histogramImage;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded);
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color p = loaded.GetPixel(i, j);

                    int red = (int)(p.R * 0.393 + p.G * 0.769 + p.B * 0.189);
                    int green = (int)(p.R * 0.349 + p.G * 0.686 + p.B * 0.168);
                    int blue = (int)(p.R * 0.272 + p.G * 0.534 + p.B * 0.131);

                    if (red > 255) red = 255;
                    if (green > 255) green = 255;
                    if (blue > 255) blue = 255;
                    processed.SetPixel(i, j, Color.FromArgb(red, green, blue));
                }
            }
            pictureBox2.Image = processed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = imageB;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = imageA;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap resultImage = new Bitmap(imageA.Width, imageA.Height);
            Color mygreen = Color.FromArgb(51, 255, 5);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;
            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtract = Math.Abs(grey - greygreen);
                    if (subtract < threshold)
                        resultImage.SetPixel(x, y, backpixel);
                    else
                        resultImage.SetPixel(x, y, pixel);
                }
            }
            pictureBox3.Image = resultImage;
        }
    }
}
