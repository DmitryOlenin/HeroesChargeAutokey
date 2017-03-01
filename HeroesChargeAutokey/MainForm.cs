using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HeroesChargeAutokey.Properties;
using Timer = System.Timers.Timer;

namespace HeroesChargeAutokey
{
    public partial class MainForm : Form
    {
        private static IntPtr _handle = IntPtr.Zero; // = GetForegroundWindow();
        //private HandleRef _handleRef; //new HandleRef(this, handle);
        private const string WindowName = "BlueStacks App Player";
        private const int ShowNormal = 1;
        private Timer _tmrAll;
        private static bool _progStart, _debug, _restart = true;
        private static int _useType, _ratio, _currNum, _step, _currValue, _counter;
        private Area _opTeamRabb, _opTeamWind, _opTeamLight, _opTeamTarot, _opTeamMedusa, _opTeamNeedle, _opTeamCat, _opTeamDivine,
            _endFight, _opTeamBot, _quizOk1, _quizOk2, _startLoot, _startFight, _autoFight, _groundFight, _attackOk, _loseFight;
        private static bool [] _skipChars = new bool[8];
        private static Stopwatch _timewatch = new Stopwatch();
        private static double[] _avTime = new double[10000];
        private static string _startWait = "";

        public MainForm()
        {
            InitializeComponent();
            Reghotkey();
            MaximizeBox = false;
        }


        /// <summary>
        ///     Метод для проверки области использования.
        /// </summary>
        /// <returns></returns>
        private static bool usage_area()
        {
            var result = false;
            var title = "";

            if (GetActiveWindowTitle(IntPtr.Zero) != null) 
                title = GetActiveWindowTitle(IntPtr.Zero);

            if (title != null && title.ToLower().Contains("bluestacks"))
                result = true;

            return result;
        }

        private static string GetActiveWindowTitle(IntPtr handleInput)
        {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);

            var handleLocal = handleInput;
            if (handleInput == IntPtr.Zero) handleLocal = NativeMethods.GetForegroundWindow();

            return NativeMethods.GetWindowText(handleLocal, buff, nChars) > 0 ? buff.ToString() : null;
        }

        private void FindHandle()
        {
            _handle = IntPtr.Zero;

            _handle = NativeMethods.FindWindow(null, WindowName);

            //var processlist = Process.GetProcesses();
            //foreach (var proc in processlist)
            //    if (proc.ProcessName == @"HD-Frontend")
            //        _handle = proc.MainWindowHandle;

            if (_handle == IntPtr.Zero)
            {
                MessageBox.Show(@"Не удаётся найти окно программы.");
                return;
            }

            SetActiveWindow();

            if (usage_area() && WindowState == FormWindowState.Normal) return;
            cb_start.Checked = false;
        }

        private static void SetActiveWindow()
        {
            if (usage_area()) return;

            NativeMethods.PostMessage(_handle, NativeMethods.WmShowme, IntPtr.Zero, IntPtr.Zero);
            //(IntPtr)NativeMethods.HwndBroadcast
            NativeMethods.SetForegroundWindow(_handle);
            NativeMethods.ShowWindow(_handle, ShowNormal);
        }

        private void BlockStart()
        {
            foreach (var cb in Controls.OfType<CheckBox>() )
                if (cb.Name != "cb_start") cb.Enabled = !cb.Enabled;
            foreach (var btn in Controls.OfType<Button>())
                btn.Enabled = !btn.Enabled;
            foreach (var cmb in Controls.OfType<ComboBox>())
                cmb.Enabled = !cmb.Enabled;
        }

        private void cb_start_CheckedChanged(object sender, EventArgs e)
        {
            lb_skip.Focus();
            StopWork();
            
            BlockStart();

            if (cb_start.Checked && cb_start.Text != @"Stop")
            {
                FindHandle();
                textClear(); //Очищаем статистику при старте

                _progStart = true;
                cb_start.Text = @"Stop";
                
                ScreenCapturePre();

                _tmrAll = new Timer();
                _tmrAll.Elapsed += tmr_all_Elapsed;
                _tmrAll.Interval = 1000; 
                _tmrAll.AutoReset = false;
                _tmrAll.Enabled = true; //Запускаем таймер через интервал

                //if (!_tmrAll.Enabled) //Запускаем таймер проверок остановок
                //{
                //    new Thread(() => tmr_all_Elapsed(_tmrAll, null)).Start();
                //}

            }
            else
            {
                cb_start.Text = @"Start";
                //_progStart = false; //Отмечаем, что программа остановлена
                //if (_tmrAll == null) return;
                //_tmrAll.Dispose();
                //_tmrAll = null;
            }
        }

        private void CheckAreas()
        {

            var skipCurr = new[] { chb_skip1.Checked, chb_skip2.Checked, chb_skip3.Checked, chb_skip4.Checked, chb_skip5.Checked, chb_skip6.Checked, chb_skip7.Checked, chb_skip8.Checked };

            for (var i = 0; i < skipCurr.Length; i++)
            {
                _skipChars[i] = skipCurr[i];
            }

            _opTeamRabb = null;
            _opTeamWind = null; 
            _opTeamLight = null; 
            _opTeamTarot = null; 
            _opTeamMedusa = null; 
            _opTeamNeedle = null; 
            _opTeamCat = null;
            _opTeamDivine = null; 

            if (_skipChars[0]) _opTeamRabb = new Area(150, 800, 800, 920, 90, 115, 130, 150, 215, 235, 4); //Заяц - 4 точки //95 -> 90
            if (_skipChars[1]) _opTeamWind = new Area(150, 800, 800, 920, 200, 230, 150, 185, 120, 145, 4); //Ветерок - 4 точки
            if (_skipChars[2]) _opTeamLight = new Area(150, 800, 800, 920, 225, 245, 190, 215, 110, 135, 4); //(4 точки глаза Несущего) 225-245, 190-215, 110-135
            if (_skipChars[3]) _opTeamTarot = new Area(150, 800, 800, 920, 60, 70, 50, 65, 65, 80, 4); //(Таро, фиолетовая морда) 225-245, 190-215, 110-135
            if (_skipChars[4]) _opTeamMedusa = new Area(150, 800, 800, 920, 135, 170, 176, 205, 165, 200, 4); //(Медуза, серая морда)  // 70, 90, 35, 55, 45, 75 - красный глаз
            if (_skipChars[5]) _opTeamNeedle = new Area(150, 800, 800, 920, 225, 245, 190, 215, 205, 235, 4); // (Игла, розовое лицо) //225, 245, 195, 215, 215, 235
            if (_skipChars[6]) _opTeamCat = new Area(150, 800, 800, 920, 50, 65, 45, 60, 90, 110, 4); // Фиолетовая морда Кисы 50-65, 45-60, 90-110
            if (_skipChars[7]) _opTeamDivine = new Area(150, 800, 800, 920, 75, 105, 55, 75, 85, 105, 4); // (Диван, фиолетовое лицо) //75 - 105, 55 - 75, 85 - 105
            
            _opTeamBot = new Area(166, 634, 505, 708, 190, 255, 0, 20, 0, 20, 2); //(2 точки красные в нике противника)
            _startLoot = new Area(1530, 842, 1696, 890, 239, 255, 240, 250, 220, 230, 3); //Запуск грабежа (белые точки) - 239
            _startFight = new Area(1556, 897, 1656, 998, 251, 255, 165, 175, 15, 25, 4); //Запуск боя (оранжевые мечи)   - 251
            _autoFight = new Area(1740, 894, 1848, 961, 40, 55, 135, 145, 0, 10, 4); //Зелёный автобой                   - 40
            _groundFight = new Area(10, 700, 20, 704, 136, 146, 85, 95, 35, 45, 3); //Оранжевое поле после начала боя 135-155 90-110 35-55 (135-145, 85-95, 35-45)
            //_botCheck = new Area(1290, 764, 1350, 800, 241, 255, 240, 250, 220, 230, 1);

            _attackOk = new Area(937, 666, 1005, 706, 241, 255, 230, 240, 190, 210, 3); //Кремовая кнопка ОК на надписи 'Вас атакуют' - 250, 234, 198
            _endFight = new Area(1595, 830, 1707, 930, 253, 255, 165, 175, 15, 25, 4); //Оранжевая стрелка конца боя
            _loseFight = new Area(1725, 914, 1755, 924, 253, 255, 215, 225, 120, 130, 4); //// Жёлтый кусок неудачного боя 1725, 914, 1755, 924 // 253, 255, 215, 225, 120, 130
            _quizOk1 = new Area(1285, 761, 1357, 804, 241, 255, 240, 250, 220, 230, 3); //OK1: 1285, 761, 1357, 804 
            _quizOk2 = new Area(925, 761, 997, 804, 241, 255, 240, 250, 220, 230, 3); //OK2: 925, 761, 997, 804
            //_bad_conn = null;

            if (_ratio != 1) return;
            if (_skipChars[0]) _opTeamRabb = new Area(90, 440, 430, 506, 120, 145, 160, 210, 230, 255, 3); // 3 точки Заяца на другом соотношении сторон 
            if (_skipChars[1]) _opTeamWind = new Area(90, 440, 430, 506, 200, 255, 180, 220, 140, 180, 3); // 3 точки Ветерка на другом соотношении сторон
            if (_skipChars[2]) _opTeamLight = new Area(90, 440, 430, 506, 200, 255, 164, 204, 115, 135, 3); // 3 точки Несущего на другом соотношении сторон
            if (_skipChars[3]) _opTeamTarot = new Area(90, 440, 430, 506, 180, 210, 50, 70, 55, 95, 3);  //Красная шляпа Таро, 180-210, 50-70, 55-95
            if (_skipChars[4]) _opTeamMedusa = new Area(90, 440, 430, 506, 90, 130, 140, 180, 140, 180, 3); //3 точки Медузы 90-130, 140-180, 140-180
            if (_skipChars[5]) _opTeamNeedle = new Area(90, 440, 430, 506, 200, 245, 205, 230, 220, 240, 3); //3 точки Иглы 230-245, 205-230, 220-240
            if (_skipChars[6]) _opTeamCat = new Area(90, 440, 430, 506, 90, 110, 80, 100, 130, 160, 3); //3 точки Кошки 90, 110, 80, 100, 130, 160
            if (_skipChars[7]) _opTeamDivine = new Area(90, 440, 430, 506, 170, 200, 120, 150, 160, 190, 3); //3 точки Дивана 170, 200, 120, 150, 160, 190

            _opTeamBot = new Area(97, 354, 276, 394, 190, 255, 0, 20, 0, 20, 0); //Проверка на красного бота           - 190
            _startLoot = new Area(823, 466, 911, 492, 239, 255, 240, 250, 220, 230, 1); //Запуск грабежа (белые точки) - 239
            _startFight = new Area(840, 495, 891, 550, 251, 255, 165, 175, 15, 25, 4); //Запуск боя (оранжевые мечи)   - 251
            _autoFight = new Area(935, 494, 992, 531, 40, 55, 135, 145, 0, 10, 4); //Зелёный автобой                   - 40
            _groundFight = new Area(10, 345, 20, 347, 136, 155, 95, 105, 35, 45, 3); //Оранжевое поле после начала боя 135-155 90-110 35-55 (135-145, 85-95, 35-45)
            _attackOk = new Area(508, 373, 543, 393, 241, 255, 230, 240, 190, 210, 3); //Кремовая кнопка ОК на надписи 'Вас атакуют' - 250, 234, 198
            _loseFight = new Area(930, 505, 940, 509, 253, 255, 215, 225, 115, 130, 4); // Жёлтый кусок неудачного боя 

            _endFight = new Area(858, 456, 918, 516, 253, 255, 165, 175, 15, 25, 4); //Оранжевая стрелка конца боя
            _quizOk1 = new Area(695, 424, 730, 444, 241, 255, 240, 250, 220, 230, 1); //OK1: 1285, 761, 1357, 804 
            _quizOk2 = new Area(503, 424, 538, 444, 241, 255, 240, 250, 220, 230, 1); //OK2: 925, 761, 997, 804

            //_bad_conn = new Area(460, 372, 592, 398, 241, 255, 230, 240, 190, 210, 3); //Кремовая кнопка Повторить

        }


        private void tmr_all_Elapsed(object sender, EventArgs e)
        {
            if (!_progStart) return;
            try
            {
                //MessageBox.Show(new Form { TopMost = true }, @"Стартанули!");

                _startWait = "";
                _timewatch.Restart();
                
                if (_restart)
                    BeginInvoke((Action)(() => lb_start.Text = @"Запустили: " + DateTime.Now.ToLongTimeString()));
                _restart = false;

                if (_useType == 1)
                {
                    MousePress(_xStart, _yStart, 100, 500); //Запуск уровня
                    MousePress(_xBuy, _yBuy, 100, 500); //Покупка, если надо
                    MousePress(_xStart, _yStart, 100, 500); //Запуск уровня (ещё раз, мало ли, покупка была).
                    MousePress(_xStart, _yStart, 100, 8000); //Выбор команды
                    MousePress(_xRel, _yRel, 100, 1500); //Перезапуск уровня
                }
                else
                {
                    _step = 0;
                    //MousePress(_xWhSearch, _yWhSearch, 100, 1200); //Ищем противника

                    //while (ScreenCapture(_targetWidth, _targetHeight, _sourceWidth, _sourceHeight, 230, 255, 0, 20, 0, 20)) //Пока цвет противника красный
                    //    MousePress(_xWhSearch, _yWhSearch, 100, 1200); //Ищем снова, если нашли антибота

                    var step1 = true;


                    CurrStage(@"Этап: ", "", 1);

                    while (step1)
                    {
                        if (MessageCheck()) return; //Проверяем на таймаут и сообщения о дисконнекте или нападении

                        //SetActiveWindow(); //Если окно неактивно - активируем

                        Thread.Sleep(50);
                        //if (!chb_skip.Checked)
                        MousePress(_xWhSearch, _yWhSearch, 100, 1600); //Ищем противника (дать время после нажатия на анализ картинки) //1700

                        QuizSolution(); //Решаем викторину

                        step1 =
                         _opTeamBot.FindColor() ||
                         _opTeamRabb != null && _opTeamRabb.FindColor() ||
                         _opTeamWind != null && _opTeamWind.FindColor() ||
                         _opTeamLight != null && _opTeamLight.FindColor() ||
                         _opTeamTarot != null && _opTeamTarot.FindColor() ||
                         _opTeamMedusa != null && _opTeamMedusa.FindColor() ||
                         _opTeamCat != null && _opTeamCat.FindColor() ||
                         _opTeamNeedle != null && _opTeamNeedle.FindColor() ||
                         _opTeamDivine != null && _opTeamDivine.FindColor()
                         ;


                    }

                    CurrStage(@"Этап: ", "", 2);
                    while (_startLoot.FindColor()) //Пока белая надпись на экране - жмём
                    {
                        if (MessageCheck()) return; //Проверяем на таймаут и сообщения о дисконнекте или нападении
                        MousePress(_startLoot.X, _startLoot.Y, 500, 700); //Грабёж
                    }

                    CurrStage(@"Этап: ", "", 3);
                    while (_startFight.FindColor())
                    {
                        if (MessageCheck()) return; //Проверяем на таймаут и сообщения о дисконнекте или нападении
                        MousePress(_startFight.X, _startFight.Y, 100, 200); //Выбор команды
                        
                    }

                    CurrStage(@"Этап: ", "", 4);
                    MousePress(_autoFight.X, _autoFight.Y,1300, 50); //Выбор автобоя

                    while (!_autoFight.FindColor()) //пока НЕ зелёный
                    {
                        if (MessageCheck()) return; //Проверяем на таймаут и сообщения о дисконнекте или нападении

                        if (_groundFight != null && _groundFight.FindColor()) //Если есть кусочек оранжевой земли 
                        {
                            Thread.Sleep(50);
                            if (!_autoFight.FindColor())
                                MousePress(_autoFight.X, _autoFight.Y, 20, 50);
                        }

                        Thread.Sleep(50);

                        if (QuizSolution()) //Если как-то оказались на 4 этапе с викториной - начинаем сначала
                            return; 
                    }

                    CurrStage(@"Этап: ", "", 5);

                    //var test = true;
                    //while (test)
                    while (_autoFight.FindColor()) //пока зелёный ждём конца боя
                    {

                        if (MessageCheck()) return; //Проверяем на таймаут и сообщения о дисконнекте или нападении
                    }

                    CurrStage(@"Этап: ", "", 6);
                    //MousePress(_xWhRest, _yWhRest, 50, 50); //Перезапуск боя (+срабатывание таймера раз в секунду)


                    var endFight = false;

                    while (!endFight)
                    {

                        if (MessageCheck()) return; //Проверяем на таймаут и сообщения о дисконнекте или нападении

                        while (_endFight.FindColor())
                        {
                            MousePress(_endFight.X, _endFight.Y, 50, 50); //Перезапуск боя пока есть оранжевая стрелка
                            endFight = true;
                        }

                        var wait = "";

                        if (_timewatch.Elapsed.TotalMinutes < 5 || wait != "") continue;

                        wait = @"Ждём 2:30 с: " + _startWait;
                        BeginInvoke((Action)(() => lb_restart.Text = wait));

                    }
                    
                }
            }
            catch
            {
                //ignore
            }
            finally
            {
                if (_progStart && _tmrAll != null)
                {

                    var finishTime = _timewatch.Elapsed.TotalSeconds;
                    BeginInvoke((Action)(() => lb_prev.Text = @"Цикл " + _counter + @". Время: " + (int)finishTime + @" сек."));

                    _avTime[_counter] = finishTime; //Math.Round(finishTime, 2);
                    //var lastCycle = _avTime[_counter].ToString("0.0000");
                    //MessageBox.Show(new Form { TopMost = true }, @"Double: " + _avTime[_counter].ToString("0.0000") + @" Int: " + (int)finishTime);
                    //BeginInvoke((Action)(() => lb_test.Text = lastCycle));
                    _counter++;
                    if (_counter == 10000 || finishTime > 600)
                    {
                        _counter = 0;
                        Array.Clear(_avTime, 0, 10000);
                    }
                    else if (_counter > 1)
                    {
                        //var average = _avTime.Average().ToString("00.00");
                        var average = (_avTime.Sum() / _counter).ToString("00.00");

                        //MessageBox.Show(new Form { TopMost = true }, @"Среднее время: " + average);
                        BeginInvoke((Action)(() => lb_average.Text = @"Среднее время: " + average + @" сек."));
                    }


                    _tmrAll.Enabled = true;
                }
            }
        }

        private bool MessageCheck()
        {

            string wait;

            if (_timewatch.Elapsed.TotalMinutes > 5 && _startWait == "")
            {
                _startWait = DateTime.Now.ToLongTimeString();
                wait = @"Ждём 2:30 с: " + _startWait;
                BeginInvoke((Action) (() => lb_restart.Text = wait));
            }

            while (_attackOk != null && _attackOk.FindColor() && _progStart) //Пока сообщение о нападении или дисконнекте на экране
            {
                MousePress(_attackOk.X, _attackOk.Y, 20, 200); //OK
            }

            if (_progStart && _timewatch.Elapsed.TotalMinutes < 150) 
                return false;

            BeginInvoke((Action)(() => b_test.Text = DateTime.Now.ToLongTimeString())); //Когда перезапустили

            if (_endFight.FindColor())
                while (_endFight.FindColor())
                    MousePress(_endFight.X, _endFight.Y, 20, 200); //Перезапуск боя пока есть жёлтая полоска поражения
            else if (_loseFight != null && _loseFight.FindColor())
                while (_loseFight.FindColor())
                    MousePress(_loseFight.X, _loseFight.Y, 20, 200); //Перезапуск боя пока есть оранжевая стрелка

            _restart = true;
            return true; // Если 2:30 висим, начинаем всё сначала
        }

        /// <summary>
        /// Метод для нажатия клавиш мыши
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="waitBefore">Ожидать до нажатия (мс.)</param>
        /// <param name="waitAfter">Ожидать после нажатия (мс.)</param>
        private void MousePress(int x, int y, int waitBefore, int waitAfter)
        {
            if (!_progStart) return;

            //if (_ratio == 0)
            //{

            //if (_step < 3)
            //{

            //    //_botCheck = new Area(1290, 764, 1350, 800, 241, 255, 240, 250, 220, 230, 1);
            //    //if (_ratio == 1) _botCheck = new Area(696, 424, 728, 444, 241, 255, 240, 250, 220, 230, 1);

            //    //Проверка с числами (кнопка ОК белая)
            //    while (_botCheck.FindColor())
            //    {
            //        if (!_progStart) return; //Пока проверка идёт - ждём и не жмём.
            //        Thread.Sleep(30);
            //    }
            //}

            //botCheck = new Area(930, 764, 990, 800, 241, 255, 240, 250, 220, 230, 1);
            //while (botCheck.FindColor())
            //{
            //    //Пока проверка идёт - ждём и не жмём.
            //}

            //}

            Thread.Sleep(waitBefore);
            Cursor.Position = new Point(x, y); //Установка позиции курсора
            NativeMethods.mouse_event(MouseeventfAbsolute | MouseeventfLeftdown, x, y, 0, 0); //| MouseeventfLeftup
            Thread.Sleep(30);
            NativeMethods.mouse_event(MouseeventfAbsolute | MouseeventfLeftup, x, y, 0, 0);
            Thread.Sleep(waitAfter);
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            cb_type.SelectedIndex = 0;
            cb_ratio.SelectedIndex = 0;
            Icon = Resources.hc_icon;
            chb_debug.Checked = true;

            textClear();

            LoadSettings();

            Activate();
        }

        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            _useType = cb_type.SelectedIndex;
            _ratio = cb_ratio.SelectedIndex;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
            Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _progStart = true;
            FindHandle();
            ScreenCapturePre();

            //var bmp = Capture(_handle, 100, 100);
            //bmp.Save("test.jpg");

            //var opTeam = new Area(0, 0, 800, 920, 130, 200, 180, 220, 240, 255);
            //_opTeamBot = new Area(97, 354, 276, 394, 190, 255, 0, 20, 0, 20, 0); //Проверка на красного бота

            //MessageBox topmost - http://stackoverflow.com/questions/16105097/why-isnt-messagebox-topmost

    
            var result = _loseFight.FindColor();
            MessageBox.Show(new Form { TopMost = true }, result.ToString());


            //MessageBox.Show(Environment.OSVersion.Platform.ToString());
            //MessageBox.Show(Environment.OSVersion.Version.ToString());

            //MessageBox.Show(new Form { TopMost = true }, TestCheckColor(90, 110, 80, 100, 130, 160, 3).ToString());
            

            //if (result) MousePress(_loseFight.X, _loseFight.Y, 20, 20);
            //MessageBox.Show("Test: "+Convert.ToInt16(1024 * 0.1 - 1024 * (1 - 0.93)));
            //MessageBox.Show("0.9: " + (int) (1024 * 0.9) + "0.1: " + (int) (1024 * 0.1));

            //NumbersCheck();

            //if (_quizOk1.FindColor())
            //    MousePress(_quizOk1.X, _quizOk1.Y, 100, 700);
            //if (_quizOk2.FindColor())
            //    MousePress(_quizOk2.X, _quizOk2.Y, 100, 700);

            //var orig = new Bitmap(@"./num4.jpg");
            ////var orig = new Bitmap(@"./v.jpg");
            //var result = NumRec(orig);
            //MessageBox.Show(result);

            //_step = 1;

            //_opTeamWind = new Area(150, 800, 800, 920, 200, 230, 150, 185, 120, 145, 4);
            //var result = _opTeamWind.FindColor();
            //MessageBox.Show(result.ToString());

            //result = ScreenCapture(_targetWidth, _targetHeight, _sourceWidth, _sourceHeight, 190, 255, 0, 20, 0, 20, 1); //Красный антибот
            //MessageBox.Show(result.ToString());

            //var result = ScreenCapture(_targetWidth, _targetHeight, _sourceWidth, _sourceHeight, 190, 255, 0, 20, 0, 20, 1);
            //MessageBox.Show(result.ToString());

            //_opTeamLight = new Area(90, 440, 430, 506, 200, 255, 164, 204, 115, 135, 2); // 4 точки Несущего на другом соотношении сторон
            //var result = _opTeamLight.FindColor();
            //MessageBox.Show(result.ToString());

            //new Thread(test).Start();



            _progStart = false;

            //Проверка на белую надпись 'Грабёж'
            //var tWidth = (int)(_gameWidth * 0.12);
            //var tHeight = (int)(_gameHeight * 0.07);//09
            //var sWidth = (int)(_gameWidth * 0.77);
            //var sHeight = _ratio == 0 ? (int)(_gameHeight * 0.72) : (int)(_gameHeight * 0.68);
            //var res = ScreenCapture(_targetWidth, _targetHeight, _sourceWidth, _sourceHeight, 230, 255, 0, 20, 0, 20); // Красный цвет
            //MessageBox.Show(res.ToString());
            ////MessageBox.Show(@"Высота окна: " + _gameHeight + @". 75% от неё: " + (_gameHeight * 0.75) + @". И округлённо: " + sHeight);

        }

        private void test()
        {
            var step1 = true;

            while (step1)
            {
                if (!_progStart) return;
                _step = 1;
                Thread.Sleep(300);
                //if (!chb_skip.Checked)
                MousePress(_xWhSearch, _yWhSearch, 100, 300); //Ищем противника
                //step1 = false;
                //ScreenCapture(_targetWidth, _targetHeight, _sourceWidth, _sourceHeight, 190, 255, 0, 20, 0, 20, 1) || //Красный антибот
                var step2 = _opTeamBot.FindColor() || _opTeamRabb.FindColor() || _opTeamWind.FindColor() || _opTeamLight.FindColor(); //_ratio == 0 &&

                BeginInvoke((Action)(() => b_test.Text = step2.ToString())); 

                if (step2) Thread.Sleep(1200);

                //MessageBox.Show(step1.ToString());

                //chb_skip.Checked = false;
            }
        }

        private void chb_debug_CheckedChanged(object sender, EventArgs e)
        {
            _debug = chb_debug.Checked;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            lb_skip.Focus();
        }

    }
}
