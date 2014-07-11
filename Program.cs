using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Win32;

namespace GrayScaler
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            var sourcePath = openFileDialog.FileName;
            var source = (Bitmap)Image.FromFile(sourcePath);
            var grayscaled = MakeGrayscale(source);
            var directory = Path.GetDirectoryName(sourcePath);
            var fileName = Path.GetFileName(sourcePath);
            var saveFileDialog = new SaveFileDialog { FileName = Path.Combine(directory, "Grayscaled - " + fileName) };
            if (saveFileDialog.ShowDialog() == true)
            {
                grayscaled.Save(saveFileDialog.FileName);
            }
        }

        public static Bitmap MakeGrayscale(Bitmap source)
        {
            var grayscaled = new Bitmap(source.Width, source.Height);
            using (var graphics = Graphics.FromImage(grayscaled))
            {
                var colorMatrix = new ColorMatrix(new float[][] 
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
                var attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
            }
            return grayscaled;
        }
    }
}
