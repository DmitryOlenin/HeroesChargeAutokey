using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;

namespace HeroesChargeAutokey
{
    public partial class MainForm
    {
        // Описание метода - https://www.codeproject.com/tips/240428/work-with-bitmap-faster-with-csharp

        public class LockBitmap : IDisposable
        {
            private readonly Bitmap source;
            private IntPtr iptr = IntPtr.Zero;
            private BitmapData bitmapData;

            private bool locked = false;
            private bool unlocked = false;
            private static readonly object lockObject = new object();
            private static readonly object unlockObject = new object();

            public byte[] Pixels { get; set; }
            public int Depth { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }

            public LockBitmap(Bitmap source)
            {
                this.source = source;
                LockBits();
            }

            /// <summary>
            /// Lock bitmap data
            /// </summary>
            public void LockBits()
            {
                try
                {
                    // double-checked lock
                    if (locked)
                    {
                        return;
                    }

                    lock (lockObject)
                    {
                        if (locked)
                        {
                            return;
                        }
                        // Get width and height of bitmap
                        Width = source.Width;
                        Height = source.Height;

                        // get total locked pixels count
                        var pixelCount = Width * Height;

                        // Create rectangle to lock
                        var rect = new Rectangle(0, 0, Width, Height);

                        // get source bitmap pixel format size
                        Depth = Image.GetPixelFormatSize(source.PixelFormat);

                        // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                        if (Depth != 8 && Depth != 24 && Depth != 32)
                        {
                            throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                        }

                        // Lock bitmap and return bitmap data
                        bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                            source.PixelFormat);

                        // create byte array to copy pixel values
                        var step = Depth / 8;
                        Pixels = new byte[pixelCount * step];
                        iptr = bitmapData.Scan0;

                        // Copy data from pointer to array
                        Marshal.Copy(iptr, Pixels, 0, Pixels.Length);
                        locked = true;
                    }
                }
                catch
                {
                    //ignore
                }

            }

            /// <summary>
            /// Unlock bitmap data
            /// </summary>
            public void UnlockBits()
            {
                try
                {
                    if (unlocked)
                    {
                        return;
                    }
                    lock (unlockObject)
                    {
                        if (unlocked)
                        {
                            return;
                        }
                        // Copy data from byte array to pointer
                        Marshal.Copy(Pixels, 0, iptr, Pixels.Length);

                        // Unlock bitmap data
                        source.UnlockBits(bitmapData);
                        unlocked = true;
                    }
                }
                catch
                {
                    //ignore
                }
            }

            /// <summary>
            /// Get the color of the specified pixel
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public Color GetPixel(int x, int y)
            {
                var clr = Color.Empty;

                // Get color components count
                var cCount = Depth / 8;

                // Get start index of the specified pixel
                var i = ((y * Width) + x) * cCount;

                if (i > Pixels.Length - cCount)
                    throw new IndexOutOfRangeException();

                if (Depth == 32) // For 32 BPP get Red, Green, Blue and Alpha
                {
                    var b = Pixels[i];
                    var g = Pixels[i + 1];
                    var r = Pixels[i + 2];
                    var a = Pixels[i + 3]; // a
                    clr = Color.FromArgb(a, r, g, b);
                }

                if (Depth == 24) // For 24 BPP get Red, Green and Blue
                {
                    var b = Pixels[i];
                    var g = Pixels[i + 1];
                    var r = Pixels[i + 2];
                    clr = Color.FromArgb(r, g, b);
                }

                // For 8 BPP get color value (Red, Green and Blue values are the same)
                if (Depth == 8)
                {
                    var c = Pixels[i];
                    clr = Color.FromArgb(c, c, c);
                }
                return clr;
            }

            /// <summary>
            /// Set the color of the specified pixel
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="color"></param>
            public void SetPixel(int x, int y, Color color)
            {
                // Get color components count
                var cCount = Depth / 8;

                // Get start index of the specified pixel
                var i = ((y * Width) + x) * cCount;

                if (Depth == 32) // For 32 BPP set Red, Green, Blue and Alpha
                {
                    Pixels[i] = color.B;
                    Pixels[i + 1] = color.G;
                    Pixels[i + 2] = color.R;
                    Pixels[i + 3] = color.A;
                }

                if (Depth == 24) // For 24 BPP set Red, Green and Blue
                {
                    Pixels[i] = color.B;
                    Pixels[i + 1] = color.G;
                    Pixels[i + 2] = color.R;
                }

                // For 8 BPP set color value (Red, Green and Blue values are the same)
                if (Depth == 8)
                {
                    Pixels[i] = color.B;
                }
            }

            public void Dispose()
            {
                UnlockBits();
            }
        }


        public class Benchmark
        {
            private static DateTime startDate = DateTime.MinValue;
            private static DateTime endDate = DateTime.MinValue;

            public static TimeSpan Span { get { return endDate.Subtract(startDate); } }

            public static void Start() { startDate = DateTime.Now; }

            public static void End() { endDate = DateTime.Now; }

            public static double GetSeconds()
            {
                if (endDate == DateTime.MinValue) return 0.0;
                else return Span.TotalSeconds;
            }
        }


        public bool TestCheckColor(int rMin, int rMax, int gMin, int gMax, int bMin, int bMax, int line)
        {
            bool result = false;
            Bitmap bmp = (Bitmap)Image.FromFile("c:\\team_check.jpg");
            Benchmark.Start();
            LockBitmap lockBitmap = new LockBitmap(bmp);
            lockBitmap.LockBits();

           
            var y = lockBitmap.Height / 2; // мой вариант
            //var y = bmp.Height / 2;
            int findStart = -9, findLine = 0; // мой вариант

            

            for (int x = 0; x < lockBitmap.Width; x++)
            //for (int x = 0; x < bmp.Width; x++)
            {
                var pix = lockBitmap.GetPixel(x, y);
                //var pix = bmp.GetPixel(x, y);
                if (pix.R >= rMin && pix.R <= rMax &&
                                pix.G >= gMin && pix.G <= gMax &&
                                pix.B >= bMin && pix.B <= bMax) //200, 50, 50 - Red!
                {
                    //Thread.Sleep(1000);
                    //return true;

                    //if (x >= 331 && x <= 336) MessageBox.Show(@"Cтарт: " + findStart + @" Количество:" + findLine + @" X: " + x);

                    if (findStart == -9)
                    {
                        findStart = x;
                        findLine = 1;
                    }
                    else if (findStart == x - 1)
                    {
                        findLine++;
                        findStart = x;
                        //if (rMin == 95) bmp.Save(x + "_check.jpg");
                    }
                    //else if (findStart > -1 && findStart < x - 1)
                    //{
                    //    findLine = 0;
                    //    findStart = -9;
                    //}

                    if (findLine >= line)
                    {
                        //MessageBox.Show(@"Цвета. Координата: " + x + @" R: " + pix.R + @" G: " + pix.G + @" B: " + pix.B + @" Нашли: " + findLine);
                        result = true;
                        break;
                    }
                }
                else //Пиксель не того цвета
                {
                    findLine = 0;
                    findStart = -9;
                }

            }


            lockBitmap.UnlockBits();
            Benchmark.End();
            var seconds = Benchmark.GetSeconds();
            b_test.Text = seconds.ToString(CultureInfo.InvariantCulture);
            return result;
        }



        //static Bitmap getImageFromDXStream(int Width, int Height, SharpDX.DataStream stream)
        //{
        //    var b = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
        //    var BoundsRect = new Rectangle(0, 0, Width, Height);
        //    BitmapData bmpData = b.LockBits(BoundsRect, ImageLockMode.WriteOnly, b.PixelFormat);
        //    int bytes = bmpData.Stride * b.Height;

        //    var rgbValues = new byte[bytes * 4];

        //    // copy bytes from the surface's data stream to the bitmap stream
        //    for (int y = 0; y < Height; y++)
        //    {
        //        for (int x = 0; x < Width; x++)
        //        {
        //            stream.Seek(y * (Width * 4) + x * 4, System.IO.SeekOrigin.Begin);
        //            stream.Read(rgbValues, y * (Width * 4) + x * 4, 4);
        //        }
        //    }

        //    Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);
        //    b.UnlockBits(bmpData);
        //    return b;
        //}



    }
}
