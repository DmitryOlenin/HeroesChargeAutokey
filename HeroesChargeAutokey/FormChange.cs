using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using HeroesChargeAutokey.Properties;

namespace HeroesChargeAutokey
{
    public partial class MainForm
    {
        [Flags]
        private enum KeyModifier
        {
            None = 0
            //Alt = 1,
            //Control = 2,
            //Shift = 4,
            //WinKey = 8
        }

        /// <summary>
        ///     Метод установки глобального хоткея запуска/остановки
        /// </summary>
        private void Reghotkey()
        {
            //FKeys = new int[11]
            var fKey = Keys.F11.GetHashCode();

            var id = 0; // The id of the hotkey. 
            NativeMethods.RegisterHotKey(Handle, id, (int)KeyModifier.None, fKey);
        }

        
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            try
            {
                //const int wmSyscommand = 0x0112;
                //const int scMinimize = 0xF020;

                //if (m.Msg == wmSyscommand)
                //    if (m.WParam.ToInt32() == scMinimize)
                //    {
                //        if (Location.X > 0) Settings.Default.pos_x = Location.X; //14.05.2015
                //        if (Location.Y > 0) Settings.Default.pos_y = Location.Y;
                //        if (Location.X > 0 || Location.Y > 0) Settings.Default.Save();

                //        if (pan_opt.Visible || _optClick == 1) b_opt_Click(null, null);
                //        error_not(2); //17.04.2015
                //        error_select();
                //        if (!_startMain || !_startOpt) // this.WindowState = FormWindowState.Minimized;
                //        {
                //            m.Result = IntPtr.Zero;
                //            error_show(2);
                //            return;
                //        }
                //    }

                //if (m.Msg == NativeMethods.WmShowme)
                //    ShowMe();

                base.WndProc(ref m);

                if (m.Msg != 0x0312) return;
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                     * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

                //var key = (Keys) (((int) m.LParam >> 16) & 0xFFFF); // The key of the hotkey that was pressed.
                //var modifier = (KeyModifier) ((int) m.LParam & 0xFFFF); // The modifier of the hotkey that was pressed.
                //var id = m.WParam.ToInt32(); // The id of the hotkey that was pressed.


                cb_start.Checked = !cb_start.Checked;

            }
            catch
            {
                // ignored
            }
        }

        private void Exit() //11.01.2016 //object sender, EventArgs e
        {
            StopWork();
            NativeMethods.UnregisterHotKey(Handle, 0);  
        }

        private void StopWork()
        {
            _progStart = false;

            if (_windowDc != IntPtr.Zero) //16.10.2015
            {
                NativeMethods.ReleaseDC(_handle, _windowDc); //IntPtr.Zero
                _windowDc = IntPtr.Zero;
            }

            //if (_handle != IntPtr.Zero) //06.05.2016
            //{
            //    //NativeMethods.CloseHandle(_handle); //Нет необходимости закрывать хендл окна 
            //    //https://social.msdn.microsoft.com/Forums/windowsdesktop/en-US/3c26ca9e-edba-4aa1-974d-182c18e0720c/findwindow-closehandle-invalid-handle-exception?forum=windowssdk

            //    _handle = IntPtr.Zero;
            //}

            if (_tmrAll == null) return;
            _tmrAll.Dispose();
            _tmrAll = null;
        }

        private void CurrStage(string str1, string str2, int currStep=0)
        {
            if (currStep != 0)
            {
                _step = currStep;
                BeginInvoke((Action)(() => lb_current.Text = str1 + _step));
            }
        }

        private void textClear()
        {
            lb_start.Text = "";
            lb_prev.Text = "";
            lb_average.Text = "";
            lb_current.Text = "";
            lb_restart.Text = "";

            _counter = 0;
            Array.Clear(_avTime, 0, 10000);
        }

        private void LoadSettings()
        {
            var skip = new[]
            {
                Settings.Default.shb_skip1, Settings.Default.shb_skip2, Settings.Default.shb_skip3,
                Settings.Default.shb_skip4, Settings.Default.shb_skip5, Settings.Default.shb_skip6, 
                Settings.Default.shb_skip7, Settings.Default.shb_skip8
            };

            var checkSkip = new[] { chb_skip1, chb_skip2, chb_skip3, chb_skip4, chb_skip5, chb_skip6, chb_skip7, chb_skip8 };

            for (var i = 0; i < skip.Length; i++)
            {
                checkSkip[i].Checked = skip[i];
            }

            StartPosition = FormStartPosition.Manual;
            if (Settings.Default.locationX > 0 && Settings.Default.locationY > 0)
                Location = new Point(Settings.Default.locationX, Settings.Default.locationY); 
        }

        private void SaveSettings()
        {
            Settings.Default.shb_skip1 = chb_skip1.Checked;
            Settings.Default.shb_skip2 = chb_skip2.Checked;
            Settings.Default.shb_skip3 = chb_skip3.Checked;
            Settings.Default.shb_skip4 = chb_skip4.Checked;
            Settings.Default.shb_skip5 = chb_skip5.Checked;
            Settings.Default.shb_skip6 = chb_skip6.Checked;
            Settings.Default.shb_skip7 = chb_skip7.Checked;
            Settings.Default.shb_skip8 = chb_skip8.Checked;

            Settings.Default.locationX = Location.X;
            Settings.Default.locationY = Location.Y;

            Settings.Default.Save();
        }
    
    }
}
