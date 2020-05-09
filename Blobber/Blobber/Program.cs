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
            Graphics OriginalGraphics = Graphics.FromImage(OriginalBitmap);

            int Width = OriginalBitmap.Width;
            int Height = OriginalBitmap.Height;
            
            Bitmap BlobBitmap = new Bitmap(Width, Height);
            Graphics BlobGraphics = Graphics.FromImage(BlobBitmap);
#if false
            for(int i = 0; i < Width; i++)
            {
                for(int u = 0; u < Height; u++)
                {
                    BlobBitmap.SetPixel(i, u, OriginalBitmap.GetPixel(i, u));
                }
            }
#else
            BitmapData OriginalData = OriginalBitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, OriginalBitmap.PixelFormat);
            BitmapData BlobData = BlobBitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, BlobBitmap.PixelFormat);

            int size = Math.Abs(OriginalData.Stride) * OriginalData.Height;
            byte[] Raw = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(OriginalData.Scan0, Raw, 0, size);

            int ColorDepth = Raw.Length / (Width * Height * 3);
            if (ColorDepth != 1) throw new ArgumentException("");

            Color[,] Image = new Color[Width, Height];
            for(int i = 0; i < Height; i++)
            {
                for(int u = 0; u < Width; u++)
                {
                    int position = 3 * u + 3 * i * Width;
                    Image[u, i] = Color.FromArgb(Raw[position], Raw[position + 1], Raw[position + 2]);
                }
            }

            ///
            /// insert manipulation here
            ///

            byte[] BlobRaw = new byte[size];
            for(int i = 0; i < Height; i++)
            {
                for(int u = 0; u < Width; u++)
                {
                    int position = 3 * u + 3 * i * Width;
                    BlobRaw[position] = Image[u, i].R;
                    BlobRaw[position + 1] = Image[u, i].G;
                    BlobRaw[position + 2] = Image[u, i].B;
                }
            }

            unsafe
            {
                TypedReference tr = __makeref(BlobRaw[0]);
                IntPtr ptr = **(IntPtr**)(&tr);
                OriginalData.Scan0 = ptr;
            }

            OriginalBitmap.UnlockBits(OriginalData);
            BlobBitmap.UnlockBits(BlobData);
#endif

            BlobBitmap.Save($"{path}blobber-{file}", ImageFormat.Jpeg);
        }

        Color GetAverageColor(List<Color> colors)
        {
            return new Color();
        }
    }
}
