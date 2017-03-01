using System;
using System.Drawing;
using System.Windows.Forms;
using Tesseract;

namespace HeroesChargeAutokey
{
    public partial class MainForm
    {

        private bool QuizSolution()
        {
            if (!_quizOk1.FindColor())
                return false;

            while (_quizOk1.FindColor())
            {
                if (_ratio == 0) NumbersCheck0();
                else NumbersCheck();

                MousePress(_quizOk1.X, _quizOk1.Y, 100, 500);
            }
            while (_quizOk2.FindColor())
                MousePress(_quizOk2.X, _quizOk2.Y, 100, 500);

            return true;
        }

        private void NumbersCheck0()
        {
            //Белый цвет номера: 235, 255, 220, 240, 180, 210
            //Совсем белый цвет: 254-255 233-234 197-198 (на 1 меньше, с вариантами)
            //Не белый цвет: 1, 100, 1, 100, 1, 100
            //Жёлтый цвет (задание): 252 x 205 x 24 

            //int rMin = 254, rMax = 255, gMin = 233, gMax = 234, bMin = 197, bMax = 198;


            //var num11 = new Area(716, 448, 743, 491, rMin, rMax, gMin, gMax, bMin, bMax, 1); //Левый верхний угол //391, 257, 403, 275
            //var num12 = new Area(744, 448, 771, 491, rMin, rMax, gMin, gMax, bMin, bMax, 1);

            //var num21 = new Area(1164, 448, 1191, 491, rMin, rMax, gMin, gMax, bMin, bMax, 1); //Правый верхний угол
            //var num22 = new Area(1192, 448, 1219, 491, rMin, rMax, gMin, gMax, bMin, bMax, 1);

            //var num31 = new Area(716, 595, 743, 638, rMin, rMax, gMin, gMax, bMin, bMax, 1); //Левый нижний угол
            //var num32 = new Area(744, 595, 771, 638, rMin, rMax, gMin, gMax, bMin, bMax, 1);

            //var num41 = new Area(1164, 595, 1191, 638, rMin, rMax, gMin, gMax, bMin, bMax, 1); //Правый нижний угол
            //var num42 = new Area(1192, 595, 1219, 638, rMin, rMax, gMin, gMax, bMin, bMax, 1);


            //712, 444, 802, 494
            //615, 444, 681, 494

            //1160, 444, 1250, 494
            //1063, 444, 1129, 494

            //712, 589, 802, 639
            //615, 589, 681, 639

            //1160, 589, 1250, 639
            //1063, 589, 1129, 639

            var num11 = new Area(712, 444, 802, 494); //Левый верхний угол 
            var num12 = new Area(615, 444, 681, 494); //квадрат для галочки

            var num21 = new Area(1160, 444, 1250, 494); //Правый верхний угол
            var num22 = new Area(1063, 444, 1129, 494);

            var num31 = new Area(712, 589, 802, 639); //Левый нижний угол
            var num32 = new Area(615, 589, 681, 639);

            var num41 = new Area(1160, 589, 1250, 639); //Правый нижний угол
            var num42 = new Area(1063, 589, 1129, 639);

            var boxes = new[] { num12, num22, num32, num42 };

            var num51 = new Area(873, 297, 917, 366); //Задачка для решения  //252, 252, 205, 205, 24, 24,
            var num52 = new Area(929, 297, 949, 366, 252, 252, 205, 205, 1, 24, 1); //Сразу узкое поле, чтобы не захватывать лишнее
            var num53 = new Area(960, 297, 1004, 366); //366, 252, 252, 205, 205, 24, 24, 

            NumVic[6] = 1;

            _currNum = 99; //Знак в задании
            num52.FindColor();
            if (_currValue < 110) //минус, не плюс
            {
                NumVic[6] = 0;
                num51 = new Area(883, 297, 927, 366); //Задачка для решения //252, 252, 205, 205, 24, 24,
                num53 = new Area(951, 297, 995, 366); //252, 252, 205, 205, 24, 24,
            }


            var checkNum = new[] { num11, num21, num31, num41, num51, num53 }; //num52

            for (var i = 0; i < checkNum.Length; i++)
            {
                _currNum = i;
                checkNum[i].FindColor();
            }

            //if (Array.IndexOf(NumVic, -1) >= 0) return;

            for (var i = 0; i < 4; i++)
                if (NumVic[i] == -1) return;


            var quiz = NumVic[6] == 0 ? NumVic[4] - NumVic[5] : NumVic[4] + NumVic[5];
            var math = false;

            for (var i = 0; i < 4; i++)
            {
                if (NumVic[i] != quiz) continue;
                //MessageBox.Show(@"Сумма: " + quiz + @"Номер: " + (i + 1) + @" X = " + boxes[i].X + @" Y = " + boxes[i].Y);
                MousePress(boxes[i].X, boxes[i].Y, 100, 100);
                math = true;
                break;
            }

            if (math) return;
            var nums = new[] {NumVic[0], NumVic[1], NumVic[2], NumVic[3]};
            Array.Sort(nums);
            for (var i = 0; i < 4; i++)
            {
                if (nums[0] != NumVic[i]) continue;
                MousePress(boxes[i].X, boxes[i].Y, 100, 100);
                break;
            }

        }

        private void NumbersCheck()
        {
            //Белый цвет номера: 235, 255, 220, 240, 180, 210
            //Совсем белый цвет: 254-255 233-234 197-198 (на 1 меньше, с вариантами)
            //Не белый цвет: 1, 100, 1, 100, 1, 100
            //Жёлтый цвет (задание): 252 x 205 x 24 

            //int rMin = 254, rMax = 255, gMin = 233, gMax = 234, bMin = 197, bMax = 198;


            //386, 252, 421, 281 // 335, 252, 370, 281 //Координаты чисел и квадратов для галочки
            //626, 252, 661, 281 // 575, 252, 610, 281 // +19 пикселей на случай третьей цифры
            //386, 330, 421, 359 // 335, 330, 370, 359
            //626, 638, 661, 359 // 575, 330, 610, 359

            var num11 = new Area(386, 252, 440, 281); //Левый верхний угол 
            var num12 = new Area(335, 252, 370, 281); //квадрат для галочки

            var num21 = new Area(626, 252, 680, 281); //Правый верхний угол
            var num22 = new Area(575, 252, 610, 281);

            var num31 = new Area(386, 330, 440, 359); //Левый нижний угол
            var num32 = new Area(335, 330, 370, 359);

            var num41 = new Area(626, 330, 680, 359); //Правый нижний угол
            var num42 = new Area(575, 330, 610, 359);

            var boxes = new[] { num12, num22, num32, num42 };

            //var num11 = new Area(390, 257, 403, 276, rMin, rMax, gMin, gMax, bMin, bMax, 1); //Левый верхний угол //391, 257, 403, 275
            //var num12 = new Area(404, 257, 417, 276, rMin, rMax, gMin, gMax, bMin, bMax, 1);
            //var num21 = new Area(629, 257, 642, 276, rMin, rMax, gMin, gMax, bMin, bMax, 1); //Правый верхний угол
            //var num22 = new Area(643, 257, 656, 276, rMin, rMax, gMin, gMax, bMin, bMax, 1);
            //var num31 = new Area(390, 335, 403, 354, rMin, rMax, gMin, gMax, bMin, bMax, 1); //Левый нижний угол
            //var num32 = new Area(404, 335, 417, 354, rMin, rMax, gMin, gMax, bMin, bMax, 1);
            //var num41 = new Area(629, 335, 642, 354, rMin, rMax, gMin, gMax, bMin, bMax, 1); //Правый нижний угол
            //var num42 = new Area(643, 335, 656, 354, rMin, rMax, gMin, gMax, bMin, bMax, 1);

            //var num51 = new Area(474, 178, 496, 210, 252, 252, 205, 205, 24, 24, 1); //Задачка для решения
            //var num52 = new Area(502, 178, 514, 210, 252, 252, 205, 205, 1, 24, 1); //Сразу узкое поле, чтобы не захватывать лишнее
            //var num53 = new Area(520, 178, 542, 210, 252, 252, 205, 205, 24, 24, 1);

            var num51 = new Area(474, 178, 496, 210, 1); //Задачка для решения
            var num52 = new Area(502, 178, 514, 210, 252, 252, 205, 205, 1, 24, 1); //Сразу узкое поле, чтобы не захватывать лишнее
            var num53 = new Area(520, 178, 542, 210, 1);

            _currNum = 99; //Знак в задании
            num52.FindColor();
            NumVic[6] = 1;

            if (_currValue < 60) //минус, не плюс
            //if (NumVic[6] == 0)
            {
                NumVic[6] = 0;
                //num51 = new Area(479, 178, 501, 210, 252, 252, 205, 205, 1, 24, 1); //Задачка для решения
                //num53 = new Area(515, 178, 537, 210, 252, 252, 205, 205, 1, 24, 1);
                num51 = new Area(479, 178, 501, 210); //Задачка для решения
                num53 = new Area(515, 178, 537, 210);
            }

            var checkNum = new[] { num11, num21, num31, num41, num51, num53 }; //, num52

            for (var i = 0; i < checkNum.Length; i++)
            {
                _currNum = i;
                checkNum[i].FindColor();
            }

            //if (Array.IndexOf(NumVic, -1) >= 0) return;

            for (var i = 0; i < 4; i++)
                if (NumVic[i] == -1) return;

            var quiz = NumVic[6] == 0 ? NumVic[4] - NumVic[5] : NumVic[4] + NumVic[5];
            var math = false;

            for (var i = 0; i < 4; i++)
            {
                if (NumVic[i] != quiz) continue;
                //MessageBox.Show(@"Сумма: " + quiz + @"Номер: " + (i + 1) + @" X = " + boxes[i].X + @" Y = " + boxes[i].Y);
                MousePress(boxes[i].X, boxes[i].Y, 100, 100);
                math = true;
                break;
            }

            if (math) return;
            var nums = new[] { NumVic[0], NumVic[1], NumVic[2], NumVic[3] };
            Array.Sort(nums);
            for (var i = 0; i < 4; i++)
            {
                if (nums[0] != NumVic[i]) continue;
                MousePress(boxes[i].X, boxes[i].Y, 100, 100);
                break;
            }

        }

        private static int FindNum(int countWhite, int countBlack)
        {
            var digit = -99;

            if (countWhite == 0) digit = -1;
            else if (countWhite >= 84 && countWhite <= 110 && countBlack >= 693 && countBlack <= 725) digit = 0;
            else if (countWhite >= 37 && countWhite <= 63 && countBlack >= 908 && countBlack <= 914) digit = 1; //можно до 950 увеличить сверху и до 900 уменьшить снизу
            else if (countWhite >= 84 && countWhite <= 95 && countBlack >= 743 && countBlack <= 755) digit = 2;
            else if (countWhite >= 62 && countWhite <= 84 && countBlack >= 743 && countBlack <= 760) digit = 3;
            else if (countWhite >= 67 && countWhite <= 97 && countBlack >= 770 && countBlack <= 774) digit = 4;
            else if (countWhite >= 51 && countWhite <= 80 && countBlack >= 725 && countBlack <= 742) digit = 5; //можно до 800 увеличить и до 700 уменьшить
            else if (countWhite >= 72 && countWhite <= 106 && countBlack >= 691 && countBlack <= 714) digit = 6;
            else if (countWhite >= 31 && countWhite <= 56 && countBlack >= 858 && countBlack <= 865) digit = 7; //можно до 900 увеличить сверху и до 800 уменьшить снизу
            else if (countWhite >= 76 && countWhite <= 109 && countBlack >= 665 && countBlack <= 681) digit = 8;
            else if (countWhite >= 71 && countWhite <= 94 && countBlack >= 696 && countBlack <= 713) digit = 9;
            else digit = -2;

            return digit;

            //6 пересекается с 0
            //3 пересекается с 5

            //нет цифры - 0	1161 (проверено)
            //+ - 						//205		894 (проверено)
            //- -                             			//102 		1161 (проверено)
            //0 - 84-95-110 	693-705-725 	(6 проверки)
            //1 - 37-62-63 	908-914 	(11 проверки)
            //2 - 84-87-95 	743-755 	(11 проверки)	//584-585	1807-1810 (проверено)
            //3 - 62-75-84	747-752-760	(9 проверки)	//513-524 	1823-1825 (проверено)	//28, 23, 19
            //4 - 67-74-97	770-774         (6 проверки)	//502-508	1868-1902 (проверено)	//20 
            //5 - 51-60-80	725-733-742	(4 проверки)	//568-569	1768-1775
            //6 - 72-106 	691-714         (9 проверки)	//612-616 	1677-1681 (проверено)	//20
            //7 - 31-35-56 	858-864-865	(7 проверки)	//407-410	2141-2143		//9
            //8 - 76-92-109	665-672-681	(4 проверки)	//654 		1599 (проверено)	//27
            //9 - 71-85-94	696-702-713	(4 проверки)	
        }

        private static int PicNum(Bitmap pImage, int rMin, int rMax, int gMin, int gMax, int bMin, int bMax)
        {
            var countWhite = 0;
            var countBlack = 0;
            //var countHalfWhite = 0;

            using (var bmp = pImage)
            {
                for (var x = 0; x < bmp.Width; x++)
                {
                    for (var y = 0; y < bmp.Height; y++)
                    {
                        var pix = bmp.GetPixel(x, y);
                        if (pix.R >= rMin && pix.R <= rMax && //Если почти белый - плюсуем
                            pix.G >= gMin && pix.G <= gMax &&
                            pix.B >= bMin && pix.B <= bMax)
                            countWhite++;
                        if (pix.R >= 1 && pix.R <= 100 && //Если чёрный - плюсуем
                            pix.G >= 1 && pix.G <= 100 &&
                            pix.B >= 1 && pix.B <= 100)
                            countBlack++;
                        //if (pix.R >= rMax && pix.R <= rMax && //Если очень белый и верхняя половина - плюсуем
                        //    pix.G >= gMin && pix.G <= gMax &&
                        //    pix.B >= bMin && pix.B <= bMax && y <= bmp.Height / 2)
                        //    countHalfWhite++;
                    }
                }
                //"__" + FindNum(countWhite, countBlack) + "__" + 
                // + "_" + countHalfWhite 
                bmp.Save(_currNum + "_" + countWhite + "_" + countBlack + ".jpg");
            }

            return countWhite;
        }

        private static string NumRec (Bitmap imgsource, int flag = 0)
        {
            string ocrtext;

            using (var engine = new TesseractEngine(@"./tessdata", "rus", EngineMode.Default))
            {
                if (flag == 0) engine.SetVariable("tessedit_char_whitelist", "0123456789");
                engine.DefaultPageSegMode = flag == 0 ? PageSegMode.SingleWord : PageSegMode.SingleChar; //Одно слово или один символ

                using (var img = PixConverter.ToPix(imgsource))
                {
                    using (var page = engine.Process(img))
                    {
                        ocrtext = page.GetText();
                    }
                }
            }

            return ocrtext;
        }

    }
}
