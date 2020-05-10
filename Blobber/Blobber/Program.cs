namespace Blobber
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Runtime.CompilerServices;

    class Program
    {
        static void Main()
        {
            string path = "../../../";
            string file = "ChickenTikkaMasala.jpg";
            DoStuff(path, file);
        }

        public static unsafe void DoStuff(string path, string file)
        {
#if false
            Bitmap OriginalBitmap = new Bitmap(path + file);
            int Width = OriginalBitmap.Width;
            int Height = OriginalBitmap.Height;

            Bitmap BlobBitmap = new Bitmap(Width, Height, OriginalBitmap.PixelFormat);

            BitmapData OriginalData = OriginalBitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, OriginalBitmap.PixelFormat);
            BitmapData BlobData = BlobBitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, BlobBitmap.PixelFormat);

            int size = Math.Abs(OriginalData.Stride) * OriginalData.Height;
            byte[] Raw = new byte[size];
            Marshal.Copy(OriginalData.Scan0, Raw, 0, size);

            int ColorDepth = Raw.Length / (Width * Height * 3);
            if (ColorDepth != 1) throw new ArgumentException("");

            Color[,] Image = new Color[Width, Height];
            for (int i = 0; i < Height; i++)
            {
                for (int u = 0; u < Width; u++)
                {
                    int position = 3 * u + 3 * i * Width;
                    Image[u, i] = Color.FromArgb(Raw[position], Raw[position + 1], Raw[position + 2]);
                }
            }

            ///
            /// insert manipulation here
            ///
#endif
            using var original = new EpicImage(new Bitmap(path + file));
            using var target = new EpicImage(new Bitmap(original.Width, original.Height, original.PixelFormat));

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    var source = original[x, y];
                    target[x, y] = source;
                }
            }

            target.Bitmap.Save($"{path}blobber-{file}", ImageFormat.Jpeg);
        }

        Color GetAverageColor(ICollection<Color> colors)
        {
            return new Color();
        }
    }

    public unsafe class EpicImage : IDisposable
    {
        public readonly Bitmap Bitmap;
        public readonly BitmapData BitmapData;
        public int Width => Bitmap.Width;
        public int Height => Bitmap.Height;
        public PixelFormat PixelFormat => Bitmap.PixelFormat;
        private readonly int BytesPerPixel;
        private readonly int HeightInPixels;
        private readonly int WidthInBytes;
        private readonly byte* Pixels;

        public EpicImage(Bitmap bitmap)
        {
            Bitmap = bitmap;
            BitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            BytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            HeightInPixels = BitmapData.Height;
            WidthInBytes = BitmapData.Width * BytesPerPixel;
            Pixels = (byte*)BitmapData.Scan0;
        }

        public byte this[int idx]
        {
            get => Pixels[idx];
            set => Pixels[idx] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte* IndexOfCoord(int x, int y) => Pixels + (y * BitmapData.Stride) + (x * BytesPerPixel);

        public Color this[int x, int y]
        {
            get
            {
                var pixel = IndexOfCoord(x, y);
                return Color.FromArgb(pixel[0], pixel[1], pixel[2]);
            }
            set
            {
                var pixel = IndexOfCoord(x, y);
                pixel[0] = value.R;
                pixel[1] = value.G;
                pixel[2] = value.B;
            }
        }

        public void Dispose()
        {
            Bitmap.UnlockBits(BitmapData);
        }
    }
}
