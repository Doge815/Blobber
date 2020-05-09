using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;

namespace imageresize
{
    class Program
    {
        static void Main()
        {
            string path = "../../../";
            var file = "ChickenTikkaMasala.jpg";
            Console.WriteLine($"Loading {file}");

            FileStream OriginalStream = new FileStream(path + file, FileMode.Open, FileAccess.Read);
            Bitmap OriginalBitmap = new Bitmap(OriginalStream);
            Image OriginalImage = OriginalBitmap;
            Graphics OriginalGraphics = Graphics.FromImage(OriginalBitmap);
            
            Bitmap BlobBitmap = new Bitmap(OriginalImage.Width, OriginalImage.Height);
            Image BlobImage = BlobBitmap;
            Graphics BlobGraphics = Graphics.FromImage(BlobBitmap);

            for(int i = 0; i < OriginalImage.Width; i++)
            {
                for(int u = 0; u < OriginalImage.Height; u++)
                {
                    BlobBitmap.SetPixel(i, u, OriginalBitmap.GetPixel(i, u));
                }
            }

            BlobBitmap.Save($"{path}blobber-{file}", ImageFormat.Jpeg);
        }

        Color GetAverageColor(List<Color> colors)
        {
            return new Color();
        }
    }
}
