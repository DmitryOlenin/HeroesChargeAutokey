using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HeroesChargeAutokey
{
    public partial class MainForm
    {
        private const int MouseeventfAbsolute = 0x8000;
        private const int MouseeventfLeftdown = 0x0002;
        private const int MouseeventfLeftup = 0x0004;
        //private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //private const int MOUSEEVENTF_MOVE = 0x0001;
        //private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //private const int MOUSEEVENTF_WHEEL = 0x0800;
        //private const int MOUSEEVENTF_XDOWN = 0x0080;
        //private const int MOUSEEVENTF_XUP = 0x0100;

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {

            // ReSharper disable once UnusedMember.Local
            public Rect(int left, int top, int right, int bottom)
                : this()
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left { get; private set; }
            public int Top { get; private set; }
            public int Right { get; private set; }
            public int Bottom { get; private set; }
        }

        private static bool GetWindowActualRect(IntPtr handle, out Rect rect)
        {
            const int dwmwaExtendedFrameBounds = 9;
            var result = NativeMethods.DwmGetWindowAttribute(handle, dwmwaExtendedFrameBounds, out rect, Marshal.SizeOf(typeof(Rect)));

            return result >= 0;
        }

        private static class NativeMethods
        {

            [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
            public static extern IntPtr DeleteObject(IntPtr hDc);

            [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern bool DeleteDC(IntPtr hdc);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
            public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

            //public const int HwndBroadcast = 0xffff; //int
            public static readonly int WmShowme = RegisterWindowMessage("WM_SHOWME");

            [DllImport("user32", CharSet = CharSet.Unicode)]
            private static extern int RegisterWindowMessage(string message);

            [DllImport("Kernel32", CharSet = CharSet.Auto)]
            public static extern bool CloseHandle(IntPtr handle);

            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

            //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            //public static extern bool PostMessage(HandleRef hWnd, uint msg, IntPtr wParam, UIntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("gdi32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = false)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool BitBlt(IntPtr hDestDc, int x, int y, int nWidth, int nHeight, IntPtr hSrcDc, int xSrc, int ySrc, int dwRop);

            [DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = false)]
            public static extern IntPtr GetDC(IntPtr hwnd);

            [DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = false)]
            public static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName); 

            //[DllImport("user32.dll", CharSet = CharSet.Auto)]
            //public static extern bool GetCursorPos(ref Point lpPoint);

            //[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "MapVirtualKey")]
            //public static extern uint _MapVirtualKey(uint uCode, int uMapType);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport(@"dwmapi.dll")]
            public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out Rect pvAttribute, int cbAttribute);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);



        }

    }
}
