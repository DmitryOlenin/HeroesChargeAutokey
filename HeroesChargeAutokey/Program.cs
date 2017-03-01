using System;
using System.Windows.Forms;

namespace HeroesChargeAutokey
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.DoEvents(); 
            Application.Run(new MainForm());
        }
    }
}
