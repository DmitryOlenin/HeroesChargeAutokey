using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace HeroesChargeAutokey
{
    public partial class MainForm
    {
        private static Rect _rc, _rcAero;
        private static Rectangle _rect = Screen.PrimaryScreen.Bounds;
        private static int 
            _xStart, _yStart, _xRel, _yRel, _xBuy, _yBuy, _gameWidth, _gameHeight, _gWidth, _gHeight,
            _xWhSearch, _yWhSearch, _xWhFight, _yWhFight, _xWhChoose, _yWhChoose, _xWhAuto, _yWhAuto, _xWhRest, _yWhRest,
            _targetWidth, _targetHeight, _sourceWidth, _sourceHeight//,_aspectRatio
            ;

        private static readonly int[] NumVic = {-1,-1,-1,-1,-1,-1, -1};
        private static IntPtr _windowDc = IntPtr.Zero;
        private const int Srcopy = 13369376;//(0x00CC0020);
        private static byte[] _image1Bytes, _image2Bytes;

        private void ScreenCapturePre()
        {

            NativeMethods.GetWindowRect(_handle, ref _rc);
            _gameWidth = _rc.Right - _rc.Left;
            _gameHeight = _rc.Bottom - _rc.Top;

            _rect = new Rectangle(_rc.Left, _rc.Top, _rc.Right - _rc.Left, _rc.Bottom - _rc.Top);
            
            GetWindowActualRect(_handle, out _rcAero);

            if (_rcAero.Right - _rcAero.Left > 0 || _rcAero.Bottom - _rcAero.Top > 0)
            {
                _gWidth = (_rcAero.Right - _rcAero.Left > 0) ? _rcAero.Right - _rcAero.Left : _gameWidth;
                _gHeight = (_rcAero.Bottom - _rcAero.Top > 0) ? _rcAero.Bottom - _rcAero.Top : _gameHeight;
                _rect = new Rectangle(_rcAero.Left, _rcAero.Top, _rcAero.Right - _rcAero.Left, _rcAero.Bottom - _rcAero.Top);
            }

            if (_gameWidth == 1030)
            {
                cb_ratio.SelectedIndex = 1;
            }

            //MessageBox.Show(new Form { TopMost = true }, @"1. X: " + _gameWidth + @", Y: " + _gameHeight + @"/// 2. X: " + _gWidth + @", Y: " + _gHeight);

            CheckAreas();

            //var wid = _rect.Width;
            //var hei = _rect.Height;
            //var wid169 = (int)Math.Round((decimal)wid / 16 * 9, MidpointRounding.AwayFromZero);
            //var wid1610 = (int)Math.Round((decimal)wid / 16 * 10, MidpointRounding.AwayFromZero);
            //var wid54 = (int)Math.Round((decimal)wid / 5 * 4, MidpointRounding.AwayFromZero);
            //var wid43 = (int) Math.Round((decimal) wid / 4 * 3, MidpointRounding.AwayFromZero); 4:3 не работает нормально

            //if (wid169 == hei || wid169 == hei + 1 || wid169 == hei - 1)  //1920х1080
            //    _aspectRatio = 169;
            //else if (wid1610 == hei || wid1610 == hei + 1 || wid1610 == hei - 1)
            //    _aspectRatio = 1610;
            //else if (wid54 == hei || wid54 == hei + 1 || wid54 == hei - 1)
            //    _aspectRatio = 54;

            _xStart = _gameWidth * 82 / 100 + _rc.Left; //82.6923
            _yStart = _gameHeight * 77 / 100 + _rc.Top; //73.7463 Старт боя и выбор команды

            _xRel = _xStart;
            _yRel = _gameHeight * 52 / 100 + _rc.Top; //Перезапуск боя

            _xBuy = _gameWidth * 55 / 100 + _rc.Left; //Покупка, если необходимо
            _yBuy = _gameHeight * 59 / 100 + _rc.Top;


            _xWhSearch = _gameWidth * 67 / 100 + _rc.Left; //700 x 500 - Повторить поиск
            _yWhSearch = _gameHeight * 74 / 100 + _rc.Top;

            _xWhFight = _gameWidth * 84 / 100 + _rc.Left; //870 x 500 - Грабёж
            _yWhFight = _yWhSearch;

            _xWhChoose = _xWhFight; //870 x 550 - Выбор команды
            _yWhChoose = _gameHeight * 81 / 100 + _rc.Top;

            _xWhAuto = _gameWidth * 93 / 100 + _rc.Left; //970 x 550 - Выбор автобоя
            _yWhAuto = _yWhChoose;

            _xWhRest = _xWhFight; //870 x 500 - Выход из боя (можно 890)
            _yWhRest = _yWhFight;

            _targetWidth = (int)(_gameWidth * 0.17);
            _targetHeight = (int)(_gameHeight * 0.06);
            _sourceWidth = (int) (_gameWidth * 0.09);
            _sourceHeight = (int) (_gameHeight * 0.52);

            if (_ratio != 0) return;

            _xWhSearch = _gameWidth * 67 / 100 + _rc.Left; 
            _yWhSearch = _gameHeight * 79 / 100 + _rc.Top;

            _xWhFight = _gameWidth * 83 / 100 + _rc.Left; 
            _yWhFight = _yWhSearch;

            _xWhChoose = _xWhFight; 
            _yWhChoose = _gameHeight * 86 / 100 + _rc.Top;

            _xWhAuto = _gameWidth * 94 / 100 + _rc.Left; 
            _yWhAuto = _yWhChoose;

            _xWhRest = _xWhFight; 
            _yWhRest = _yWhFight;

            _targetWidth = (int)(_gameWidth * 0.17);
            _targetHeight = (int)(_gameHeight * 0.06);
            _sourceWidth = (int)(_gameWidth * 0.09);
            _sourceHeight = (int)(_gameHeight * 0.555);



            //MessageBox.Show(@"Размеры окна игры: " + gameWidth + @", " + gameHeight);
        }

        private static bool ScreenCapture(int tWidth, int tHeight, int sWidth, int sHeight, int rMin, int rMax, int gMin, int gMax, int bMin, int bMax, int line)
        {
            if (!_progStart) return false;

            try
            {

                if (_windowDc == IntPtr.Zero) //31.05.2016
                    _windowDc = NativeMethods.GetDC(_handle); //16.10.2015

                //_windowDc = NativeMethods.CreateCompatibleDC(_windowDc);

                if (_windowDc == IntPtr.Zero)
                {
                    MessageBox.Show(@"Не удаётся получить область окна приложения");
                    return true;
                }

                //IntPtr hMemDC = NativeMethods.CreateCompatibleDC(_windowDc);
                //IntPtr hBitmap = NativeMethods.CreateCompatibleBitmap(_windowDc, tWidth, tHeight);

                //IntPtr hOld = NativeMethods.SelectObject(hMemDC, hBitmap);
                //bool b = NativeMethods.BitBlt(hMemDC, 0, 0, tWidth, tHeight, _windowDc, sWidth, sHeight, Srcopy);
                //NativeMethods.SelectObject(hMemDC, hOld);
                //NativeMethods.DeleteDC(hMemDC);
                //NativeMethods.ReleaseDC(_handle, _windowDc);

                //using (var bmp = Image.FromHbitmap(hBitmap))
                //{
                //    NativeMethods.DeleteObject(hBitmap);

                //    if (rMin == 230)
                //        bmp.Save("red_check.jpg");
                //    else if (rMin == 240)
                //        bmp.Save("white_check.jpg");
                //    else if (rMin == 180)
                //        bmp.Save("orange_check.jpg");

                //    var y = tHeight / 2;
                //    var xFrom = tWidth / 2 - 10;
                //    var xTo = tWidth / 2 + 11;

                //    //MessageBox.Show(@"Y: " + y + @" X1: " + xFrom + @" X2: " + xTo);

                //    var count = 0;

                //    for (var x = xFrom; x < xTo; x++)
                //    {
                //        var pix = bmp.GetPixel(x, y);
                //        if (pix.R >= rMin && pix.R <= rMax &&
                //            pix.G >= gMin && pix.G <= gMax &&
                //            pix.B >= bMin && pix.B <= bMax) //200, 50, 50 - Red!
                //        {
                //            //MessageBox.Show(@"Нашли злодея!");
                //            return true;
                //        }
                //        if (pix.R + pix.G + pix.B == 0)
                //            count++;
                //    }

                //    if (count > 15)
                //        return true;                    
                //}

                //GC.Collect();

                //using (var pImage1 = new Bitmap(1040, 678, PixelFormat.Format32bppRgb)) //_gameWidth, _gameHeight
                //{
                //    using (var graph = Graphics.FromImage(pImage1))
                //    {

                //        graph.CopyFromScreen(new Point(_rc.Left, _rc.Top), Point.Empty, new Size(1040, 678));

                //        //var hdcDest = graph.GetHdc();
                //        //try
                //        //{
                //        //    NativeMethods.BitBlt(hdcDest, 0, 0, _gameWidth, _gameHeight, _windowDc, 0, 0, Srcopy);
                //        //    //0
                //        //}
                //        //finally
                //        //{
                //        //    graph.ReleaseHdc(hdcDest);
                //        //}
                //    }
                //    pImage1.Save("test111.jpg");
                //}

                //return true;

                var rectUse = new Rectangle(_rect.Left + sWidth, _rect.Top + sHeight, tWidth, tHeight);
                var colorsR = new[] { 95, 241, 200, 0, 1, 2, 252, 120, 225, 253, 190, 239, 251, 40, 60, 135, 90, 180, 50, 75, 136, 170 }; 

                using (var pImage = new Bitmap(tWidth, tHeight)) //PixelFormat.Format32bppRgb
                {
                    using (var graph = Graphics.FromImage(pImage))
                    {
                        if (colorsR.Contains(rMin))
                        {
                            graph.CopyFromScreen(rectUse.Location, new Point(0, 0), rectUse.Size);
                            //var hDc = graph.GetHdc();
                            //try { NativeMethods.PrintWindow(_handle, hDc, (uint)0); }
                            //finally { graph.ReleaseHdc(hDc); }
                        }
                        else
                        {
                            //graph.CopyFromScreen(_rect.Location, new Point(0, 0), _rect.Size); //Полный размер окна приложения
                            var hdcDest = graph.GetHdc();
                            try
                            {
                                NativeMethods.BitBlt(hdcDest, 0, 0, _gameWidth, _gameHeight, _windowDc, sWidth, sHeight, Srcopy); //0
                            }
                            finally
                            {
                                graph.ReleaseHdc(hdcDest);
                            }
                        }
                    }

                    //Thread.Sleep(2000);
                    //pImage.Save(@"test444.jpg");
                    //Thread.Sleep(50);

                    //if (PicDouble(pImage, rMin))
                    //    return true;

                    //if (_step == 1 && rMin == 241)
                    //{
                    //    image2Bytes = image1Bytes;
                    //    image1Bytes = null;
                    //}

                    //if (_step == 1 && image1Bytes != null && image2Bytes != null)
                    //{
                    //    if (image1Bytes == image2Bytes)
                    //    {
                    //        image1Bytes = null;
                    //        Thread.Sleep(1000); //Ждём секунду
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        image1Bytes = null;
                    //        image2Bytes = null;
                    //    }
                    //}



                    //pImage.Save("Al.jpg");
                    //MessageBox.Show(@"Cохранили1");

                    using (var bmp = new Bitmap (pImage))
                    {
                        //MessageBox.Show(@"Cохранили2");

                        if (_debug && rMin != 136) //Не сохраняем оранжевый кусок поля
                            PicSave(bmp, rMin, rMax, gMin, gMax, bMin, bMax); //

                        if (rMin == 0 || rMin == 1 || rMin == 2 || rMin == 252) //Если это анализ цифр
                            return true; //52 || rMin == 254 || rMin == 255

                        var y = tHeight / 2;
                        var xFrom = 0;//tWidth / 2 - 10;
                        var xTo = tWidth;//tWidth / 2 + 11;

                        //if (_debug) MessageBox.Show(@"Y: " + y + @" X1: " + xFrom + @" X2: " + xTo);

                        int count = 0, findStart = -9, findLine = 0;


                        for (var x = xFrom; x < xTo; x++)
                        {
                            var pix = bmp.GetPixel(x, y);
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
                                    return true;
                                }
                            }
                            else //Пиксель не того цвета
                            {
                                findLine = 0;
                                findStart = -9;
                            }

                            if (pix.R + pix.G + pix.B == 0)
                                count++;

                            //if (x >= 69 && x <=72 ) MessageBox.Show(@"Цвета. Координата: " + x + @" R: " + pix.R + @" G: " + pix.G + @" B: " + pix.B + @" Нашли: " + findLine);
                            //if (x >= 69 && x <= 72) MessageBox.Show(@"Цвета. Координата: " + x + @" R: " + pix.R + @" G: " + pix.G + @" B: " + pix.B + @" Нашли: " + findLine);
                        }

                        if (count > 15)
                            return true;
                    }
                }


            }
            catch
            {
                //ignore
            }
            //finally
            //{
            //    //NativeMethods.DeleteDC(_windowDc);
            //}
            return false;
        } //end of ScreenCapture

        private static void PicSave(Image bmpOrig, int rMin, int rMax, int gMin, int gMax, int bMin, int bMax) 
        {
            using (var bmp = new Bitmap(bmpOrig))
            {
                switch (rMin)
                {
                    case 190:
                        bmp.Save("red_check.jpg");
                        break;
                    case 240:
                        bmp.Save("white_check.jpg");
                        break;
                    case 241:
                        bmp.Save("victor_check.jpg");
                        break;
                    //case 136:
                    //    bmp.Save("orange_check.jpg");
                    //    break;
                    case 40:
                        bmp.Save("green_check.jpg");
                        break;
                    case 50: //Киса
                    case 60: //Таро
                    case 75: //Диван
                    case 90: //Заяц, Медуза*, Кошка*
                    case 120: //Заяц*
                    case 135: //Медуза
                    case 170: //Диван*
                    case 180: //Таро*
                    case 200: //Ветер, Ветер*, Несущий*, Игла*
                    case 225: //Несущий, Игла
                        bmp.Save("team_check.jpg");
                        break;
                    case 239:
                        bmp.Save("start_loot.jpg");
                        break;
                    case 251:
                        bmp.Save("start_fight.jpg");
                        break;
                    case 252:
                        _currValue = PicNum(bmp, rMin, rMax, gMin, gMax, bMin, bMax); 
                        break;
                    case 253:
                        bmp.Save("end_fight.jpg");
                        break;
                    case 0:
                    case 1:
                    case 2:
                    {
                        int num;
                        var result = NumRec(bmp, rMin);
                        if (int.TryParse(result, out num))
                            NumVic[_currNum] = num;
                        //else if (_currNum == 6 && result == "-")
                        //    NumVic[_currNum] = 0;
                        //else if (_currNum == 6 && result == "+")
                        //    NumVic[_currNum] = 1;



                        //if (_currNum < 7)
                        //    bmp.Save(@"Num_ " + _currNum + @"_val_" + NumVic[_currNum] + @".jpg");

                        //else
                            //bmp.Save(@"test.jpg");


                    }

                        break;
                }
            }
        }

        private static bool PicDouble(Image pImage, int rMin)
        {
            if (_step == 0)
            {
                _image1Bytes = null;
                _image2Bytes = null;
            }
            else if (_step == 1 && rMin == 190 && _image1Bytes == null) //Зафиксировали соперника при проверке на красное имя
                using (var mstream = new MemoryStream())
                {
                    pImage.Save(mstream, ImageFormat.Bmp); //pImage.RawFormat
                    _image1Bytes = mstream.ToArray();
                }
            else if (_step == 1 && rMin == 190 && _image1Bytes != null)
                using (var mstream = new MemoryStream())
                {
                    pImage.Save(mstream, ImageFormat.Bmp); //pImage.RawFormat
                    _image2Bytes = mstream.ToArray();

                    if (Equals(_image1Bytes, _image2Bytes))
                    {
                        Thread.Sleep(1000); //Ждём секунду
                        return true;
                    }
                    _image1Bytes = null;
                }
            return false;
        }


        //public struct SIZE
        //{
        //    public int cx;
        //    public int cy;
        //}

        ////Описание метода: http://forums.devshed.com/programming-42/capturing-windows-content-265813.html
        //public static Bitmap Capture(IntPtr hwnd, int width, int height)
        //{
        //    //Size contains the size of the screen
        //    SIZE size;

        //    //hBitmap contains the handle to the bitmap
        //    IntPtr hBitmap;

        //    //Get handle to the desktop device context
        //    IntPtr hDC = NativeMethods.GetDC(hwnd);

        //    //Device context in memory for screen device context

        //    IntPtr hMemDC = NativeMethods.CreateCompatibleDC(hDC);

        //    //Pass SM_CXSCREEN to GetSystemMetrics to get the X coordinates 
        //    //of the screen
        //    size.cx = width;
        //    //As above but get Y corrdinates of the screen
        //    size.cy = height;

        //    //Create a compatiable bitmap of the screen size using the 
        //    //screen device context
        //    hBitmap = NativeMethods.CreateCompatibleBitmap(
        //        hDC, size.cx, size.cy);

        //    //Cannot check of IntPtr is null so check against zero instead
        //    if (hBitmap != IntPtr.Zero)
        //    {
        //        //Select the compatiable bitmap in the memeory and keep a 
        //        //refrence to the old bitmap
        //        IntPtr hOld = NativeMethods.SelectObject(hMemDC, hBitmap);
        //        //Copy bitmap into the memory device context
        //        bool b = NativeMethods.BitBlt(hMemDC, 0, 0,
        //            size.cx, size.cy, hDC, 0, 0, Srcopy);
        //        //Select the old bitmap back to the memory device context
        //        NativeMethods.SelectObject(hMemDC, hOld);
        //        //Delete memory device context
        //        NativeMethods.DeleteDC(hMemDC);
        //        //Release the screen device context
        //        NativeMethods.ReleaseDC(hwnd, hDC);
        //        //Create image
        //        Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
        //        //Release memory to avoid leaks
        //        NativeMethods.DeleteObject(hBitmap);
        //        //Run garbage collector manually
        //        GC.Collect();
        //        //Return the bitmap
        //        return bmp;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}



        private class Area
        {
            private int _rMin, _rMax, _gMin, _gMax, _bMin, _bMax, _tWidth, _tHeight, _sWidth, _sHeight, _line;
            public int X { get; private set; }
            public int Y { get; private set; }

            public Area(int x1, int y1, int x2, int y2, int r1 = 0, int r2 = 0, int g1 = 0, int g2 = 0, int b1 = 0, int b2 = 0, int line = 0)
            {
                _line = line;

                _rMin = r1;
                _rMax = r2;
                _gMin = g1;
                _gMax = g2;
                _bMin = b1;
                _bMax = b2;

                _tWidth = x2 - x1;
                _tHeight = y2 - y1;
                _sWidth = x1;
                _sHeight = y1;

                X = _rect.Left + _sWidth + _tWidth / 2 ;//+ _rc.Left;
                Y = _rect.Top + _sHeight + _tHeight / 2 ;// + _rc.Top;
            }

            public bool FindColor ()
            {
                return ScreenCapture(_tWidth, _tHeight, _sWidth, _sHeight, _rMin, _rMax, _gMin, _gMax, _bMin, _bMax, _line);
            }
        } // end of Area

    }
}
